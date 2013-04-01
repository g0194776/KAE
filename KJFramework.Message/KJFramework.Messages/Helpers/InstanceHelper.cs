using System;
using KJFramework.Messages.Analysers;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Engine;
using KJFramework.Messages.Exceptions;
using KJFramework.Messages.Objects;
using KJFramework.Messages.TypeProcessors;
using KJFramework.Messages.TypeProcessors.Maps;

namespace KJFramework.Messages.Helpers
{
    /// <summary>
    ///     实例帮助器
    /// </summary>
    internal static class InstanceHelper
    {
        #region Methods

        /// <summary>
        ///     设置字段实例
        /// </summary>
        /// <param name="instance">对象实例</param>
        /// <param name="analyseResult">字段临时解析结构</param>
        /// <param name="data">元数据</param>
        /// <param name="offset">元数据偏移</param>
        /// <param name="length">元数据可用长度</param>
        public static void SetInstance(object instance, GetObjectAnalyseResult analyseResult, byte[] data, int offset, int length)
        {
            GetObjectAnalyseResult analyze = analyseResult;
            //热处理判断
            if (analyze.HasCacheFinished)
            {
                analyze.CacheProcess(instance, analyseResult, data, offset, length);
                return;
            }

            #region 普通类型判断

            IIntellectTypeProcessor intellectTypeProcessor = IntellectTypeProcessorMapping.Instance.GetProcessor(analyze.Attribute.Id) ??
                                                                                    IntellectTypeProcessorMapping.Instance.GetProcessor(analyze.Property.PropertyType);
            if (intellectTypeProcessor != null)
            {
                //添加热缓存
                IIntellectTypeProcessor processor = intellectTypeProcessor;
                if (intellectTypeProcessor.SupportUnmanagement) analyze.CacheProcess = processor.Process;
                else analyze.CacheProcess = delegate(object innerInstance, GetObjectAnalyseResult innerAnalyseResult, byte[] parameter, int position, int len) { processor.Process(analyze.Attribute, parameter); };
                analyze.CacheProcess(instance, analyseResult, data, offset, length);
                analyze.HasCacheFinished = true;
                return;
            }

            #endregion

            #region 枚举类型判断

            //枚举类型
            if (analyze.Property.PropertyType.IsEnum)
            {
                Type enumType = Enum.GetUnderlyingType(analyze.Property.PropertyType);
                intellectTypeProcessor = IntellectTypeProcessorMapping.Instance.GetProcessor(enumType);
                if (intellectTypeProcessor == null) throw new System.Exception("Cannot support this enum type! #type: " + analyze.Property.PropertyType);
                //添加热处理
                IIntellectTypeProcessor processor = intellectTypeProcessor;
                if (intellectTypeProcessor.SupportUnmanagement) analyze.CacheProcess = processor.Process;
                else analyze.CacheProcess = delegate(object innerInstance, GetObjectAnalyseResult innerAnalyseResult, byte[] parameter, int position, int len) { processor.Process(analyze.Attribute, parameter); };
                analyze.CacheProcess(instance, analyseResult, data, offset, length);
                analyze.HasCacheFinished = true;
                return;
            }

            #endregion

            #region 可空类型判断

            Type innerType;
            if ((innerType = Nullable.GetUnderlyingType(analyze.Property.PropertyType)) != null)
            {
                intellectTypeProcessor = IntellectTypeProcessorMapping.Instance.GetProcessor(innerType);
                if (intellectTypeProcessor != null)
                {
                    //添加热缓存
                    IIntellectTypeProcessor processor = intellectTypeProcessor;
                    if (intellectTypeProcessor.SupportUnmanagement) analyze.CacheProcess = processor.Process;
                    else analyze.CacheProcess = delegate(object innerInstance, GetObjectAnalyseResult innerAnalyseResult, byte[] parameter, int position, int len) { processor.Process(analyze.Attribute, parameter); };
                    analyze.CacheProcess(instance, analyseResult, data, offset, length);
                    analyze.HasCacheFinished = true;
                    return;
                }
                throw new System.Exception("Cannot find compatible processor, #type: " + analyze.Property.PropertyType);
            }

            #endregion

            #region 智能类型的判断

            //智能对象的判断
            if (analyze.Property.PropertyType.IsClass && analyze.Property.PropertyType.GetInterface(Consts.IntellectObjectFullName) != null)
            {
                //添加热缓存
                analyze.CacheProcess = delegate(Object innerInstance, GetObjectAnalyseResult innerAnalyseResult, byte[] parameter, int position, int len)
                    {
                        innerAnalyseResult.SetValue(innerInstance, IntellectObjectEngine.GetObject<IntellectObject>(analyze.Property.PropertyType, parameter, position, len));
                    };
                analyze.CacheProcess(instance, analyseResult, data, offset, length);
                analyze.HasCacheFinished = true;
                return;
            }

            #endregion

            #region 数组的判断

            if (analyze.Property.PropertyType.IsArray)
            {
                Type elementType = analyze.Property.PropertyType.GetElementType();
                VT vt = FixedTypeManager.IsVT(elementType);
                //VT type.
                if (vt != null)
                {
                    #region VT type array processor.

                    IIntellectTypeProcessor arrayProcessor = ArrayTypeProcessorMapping.Instance.GetProcessor(analyseResult.Property.PropertyType);
                    //special optimize.
                    if (arrayProcessor != null)
                    {
                        analyze.CacheProcess = arrayProcessor.Process;
                        analyze.CacheProcess(instance, analyseResult, data, offset, length);
                    }
                    //normally process.
                    else throw new DefineNoMeaningException(string.Format(ExceptionMessage.EX_VT_FIND_NOT_PROCESSOR, analyze.Attribute.Id, analyze.Property.Name, analyze.Property.PropertyType));

                    #endregion
                }
                else if (elementType.IsSubclassOf(typeof(IntellectObject)))
                {
                    #region IntellectObject type array processor.

                    //add HOT cache.
                    analyze.CacheProcess = delegate(Object innerInstance, GetObjectAnalyseResult innerAnalyseResult, byte[] parameter, int position, int len)
                    {
                        int innerOffset = position;
                        int chunkSize = position + len;
                        int arrLen = BitConverter.ToInt32(parameter, innerOffset);
                        if (arrLen == 0)
                        {
                            innerAnalyseResult.SetValue(innerInstance, Activator.CreateInstance(analyze.Property.PropertyType, 0));
                            return;
                        }
                        innerOffset += 4;
                        IntellectObject[] array = (IntellectObject[])Activator.CreateInstance(analyze.Property.PropertyType, arrLen);
                        int arrIndex = 0;
                        short size;
                        do
                        {
                            size = BitConverter.ToInt16(parameter, innerOffset);
                            innerOffset += 2;
                            if ((parameter.Length - innerOffset) < size)
                                throw new System.Exception("Illegal remaining binary data length!");
                            //use unmanagement method by default.
                            if (size == 0) array[arrIndex] = null;
                            else array[arrIndex] = IntellectObjectEngine.GetObject<IntellectObject>(elementType, parameter, innerOffset, size);
                            innerOffset += size;
                            arrIndex++;
                        } while (innerOffset < parameter.Length && innerOffset < chunkSize);
                        innerAnalyseResult.SetValue(innerInstance, array);
                    };

                    #endregion
                }
                else if (!(elementType == typeof(string)) && elementType.IsSerializable)
                    throw new NotSupportedException(string.Format(ExceptionMessage.EX_NOT_SUPPORTED_VALUE, analyseResult.Attribute.Id, analyseResult.Property.Name, analyseResult.Property.PropertyType));
                else if (elementType == typeof(string))
                {
                    #region Any types if it can get the processor.
                    intellectTypeProcessor = ArrayTypeProcessorMapping.Instance.GetProcessor(analyze.Property.PropertyType);
                    if (intellectTypeProcessor == null) throw new System.Exception("Cannot support this array element type processor! #type: " + elementType);
                    //Add hot cache.
                    analyze.CacheProcess = intellectTypeProcessor.Process;
                    #endregion
                }
                else throw new NotSupportedException(string.Format(ExceptionMessage.EX_NOT_SUPPORTED_VALUE, analyseResult.Attribute.Id, analyseResult.Property.Name, analyseResult.Property.PropertyType));
                analyze.HasCacheFinished = true;
                analyze.CacheProcess(instance, analyseResult, data, offset, length);
                return;
            }

            #endregion

            #region 第三方要求序列化类的判断

            if (analyze.Property.PropertyType.IsSerializable)
            {
                intellectTypeProcessor = IntellectTypeProcessorMapping.Instance.GetProcessor(typeof(ClassSerializeObject));
                IIntellectTypeProcessor processor = intellectTypeProcessor;
                analyze.CacheProcess = delegate(Object innerInstance,GetObjectAnalyseResult innerAnalyseResult,byte[] parameter, int position, int len) { processor.Process(analyze.Attribute, parameter); };
                analyze.CacheProcess(instance, analyseResult, data, offset, length);
                analyze.HasCacheFinished = true;
                return;
            }

            #endregion

            throw new System.Exception("Cannot support this data type: " + analyze.Property.PropertyType);
        }

        #endregion
    }
}