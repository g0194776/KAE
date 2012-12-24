using KJFramework.Messages.Analysers;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Exceptions;
using KJFramework.Messages.Helpers;
using KJFramework.Messages.Objects;
using KJFramework.Messages.Proxies;
using KJFramework.Messages.TypeProcessors;
using KJFramework.Messages.TypeProcessors.Maps;
using KJFramework.Tracing;
using System;
using System.Collections.Generic;

namespace KJFramework.Messages.Engine
{
    /// <summary>
    ///     ���ܶ������棬�ṩ����صĻ���������
    /// </summary>
    public sealed class IntellectObjectEngine
    {
        #region Members

        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(IntellectObjectEngine));

        #endregion

        #region Methods

        /// <summary>
        ///     Ԥ��һ�����ܶ���
        /// </summary>
        /// <param name="intellectObject">��ҪԤ�ȵ����ܶ���</param>
        public static void Preheat(IIntellectObject intellectObject)
        {
            try
            {
                if (intellectObject == null) throw new ArgumentNullException("intellectObject");
                Analyser.ToBytesAnalyser.Analyse(intellectObject);
                Analyser.GetObjectAnalyser.Analyse(intellectObject.GetType());
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                throw;
            }
        }

        #endregion

        #region ͨ�ö�������Ϣ����

        /// <summary>
        ///     ��һ�����ܶ���ת��Ϊ������Ԫ����
        /// </summary>
        /// <param name="obj">���ܶ���</param>
        /// <returns>���ض�����Ԫ����</returns>
        /// <exception cref="PropertyNullValueException">�ֶ���ص�Attribute.IsRequire = true, ���Ҹ��ֶε�ֵΪnull</exception>
        /// <exception cref="NotSupportedException">ϵͳ��֧�ֵ����л�����</exception>
        /// <exception cref="DefineNoMeaningException">������������ֶ�Attributeֵ</exception>
        /// <exception cref="MethodAccessException">����Ȩ�޶������</exception>
        /// <exception cref="Exception">�ڲ�����</exception>
        public static byte[] ToBytes(IIntellectObject obj)
        {
            if (obj == null) return null;
            MemoryAllotter.Instance.Initialize();
            IMemorySegmentProxy proxy = null;
            try
            {
                proxy = MemorySegmentProxyFactory.Create();
                ToBytes(obj, proxy);
                return proxy.GetBytes();
            }
            catch
            {
                if (proxy != null) proxy.Dispose();
                throw;
            }
        }

        /// <summary>
        ///     ��һ�����ܶ���ת��Ϊ������Ԫ����
        /// </summary>
        /// <param name="obj">���ܶ���</param>
        /// <param name="proxy">�ڴ�δ�����</param>
        private static void ToBytes(IIntellectObject obj, IMemorySegmentProxy proxy)
        {
            //��ȡ���ܶ����е��������ԣ�������Id������
            ToBytesAnalyseResult[] properties = Analyser.ToBytesAnalyser.Analyse(obj);
            if (properties.Length == 0) return;
            MemoryPosition wrapperStartPosition = proxy.GetPosition();
            proxy.Skip(4U);
            IIntellectTypeProcessor intellectTypeProcessor;
            for (int l = 0; l < properties.Length; l++ )
            {
                ToBytesAnalyseResult property = properties[l];
                //�ȼ����ȫ�������
                if (property.HasCacheFinished)
                {
                    property.CacheProcess(proxy, property.Attribute, property, obj, false, false);
                    continue;
                }

                #region ��ͨ�����ж�

                intellectTypeProcessor = IntellectTypeProcessorMapping.Instance.GetProcessor(property.Attribute.Id) ??
                                                       IntellectTypeProcessorMapping.Instance.GetProcessor(property.Property.PropertyType);
                if (intellectTypeProcessor != null)
                {
                    //����Ȼ���
                    IIntellectTypeProcessor processor = intellectTypeProcessor;
                    property.CacheProcess = processor.Process;
                    property.CacheProcess(proxy, property.Attribute, property, obj, false, false);
                    property.HasCacheFinished = true;
                    continue;
                }

                #endregion

                #region ö�������ж�

                //ö������
                if (property.Property.PropertyType.IsEnum)
                {
                    //��ȡö������
                    Type enumType = Enum.GetUnderlyingType(property.Property.PropertyType);
                    intellectTypeProcessor = IntellectTypeProcessorMapping.Instance.GetProcessor(enumType);
                    if (intellectTypeProcessor == null)
                        throw new NotSupportedException(string.Format(ExceptionMessage.EX_NOT_SUPPORTED_VALUE, property.Attribute.Id, property.Property.Name, property.Property.PropertyType));
                    //����Ȼ���
                    IIntellectTypeProcessor processor = intellectTypeProcessor;
                    property.CacheProcess = processor.Process;
                    property.CacheProcess(proxy, property.Attribute, property, obj, false, false);
                    property.HasCacheFinished = true;
                    continue;
                }

                #endregion

                #region �ɿ������ж�

                Type innerType;
                if ((innerType = Nullable.GetUnderlyingType(property.Property.PropertyType)) != null)
                {
                    intellectTypeProcessor = IntellectTypeProcessorMapping.Instance.GetProcessor(innerType);
                    if (intellectTypeProcessor != null)
                    {
                        //����Ȼ���
                        IIntellectTypeProcessor processor = intellectTypeProcessor;
                        property.CacheProcess = delegate(IMemorySegmentProxy innerProxy, IntellectPropertyAttribute innerAttribute, ToBytesAnalyseResult innerAnalyseResult, object innerTarget, bool innerIsArrayElement, bool innerNullable)
                        {
                            processor.Process(innerProxy, innerAttribute, innerAnalyseResult, innerTarget, innerIsArrayElement, true);
                        };
                        property.CacheProcess(proxy, property.Attribute, property, obj, false, true);
                        property.HasCacheFinished = true;
                        continue;
                    }
                    throw new NotSupportedException(string.Format(ExceptionMessage.EX_NOT_SUPPORTED_VALUE, property.Attribute.Id, property.Property.Name, property.Property.PropertyType));
                }

                #endregion

                #region ���ܶ��������ж�

                if (property.Property.PropertyType.GetInterface(Consts.IntellectObjectFullName) != null)
                {
                    //����Ȼ���
                    property.CacheProcess = delegate(IMemorySegmentProxy innerProxy, IntellectPropertyAttribute innerAttribute, ToBytesAnalyseResult innerAnalyseResult, object innerTarget, bool innerIsArrayElement, bool innerNullable)
                    {
                        IntellectObject innerIntellectObj = innerAnalyseResult.GetValue<IntellectObject>(innerTarget);
                        if (innerIntellectObj == null) return;
                        innerProxy.WriteByte((byte)innerAttribute.Id);
                        MemoryPosition startPos = innerProxy.GetPosition();
                        innerProxy.Skip(4);
                        ToBytes(innerIntellectObj, innerProxy);
                        MemoryPosition endPos = innerProxy.GetPosition();
                        innerProxy.WriteBackInt32(startPos, MemoryPosition.CalcLength(innerProxy.SegmentCount, startPos, endPos) - 4);
                    };
                    property.CacheProcess(proxy, property.Attribute, property, obj, false, false);
                    property.HasCacheFinished = true;
                    continue;
                }

                #endregion

                #region ������ж�

                if (property.Property.PropertyType.IsArray)
                {
                    if (!property.Property.PropertyType.HasElementType)
                        throw new NotSupportedException(string.Format(ExceptionMessage.EX_NOT_SUPPORTED_VALUE, property.Attribute.Id, property.Property.Name, property.Property.PropertyType));
                    Type elementType = property.Property.PropertyType.GetElementType();
                    VT vt = FixedTypeManager.IsVT(elementType);
                    //special optimize.
                    IIntellectTypeProcessor arrayProcessor = ArrayTypeProcessorMapping.Instance.GetProcessor(property.Property.PropertyType);
                    if (arrayProcessor != null) property.CacheProcess = arrayProcessor.Process;
                    //is VT, but cannot find special processor.
                    else if (vt != null)
                        throw new NotSupportedException(string.Format(ExceptionMessage.EX_NOT_SUPPORTED_VALUE, property.Attribute.Id, property.Property.Name, property.Property.PropertyType));
                    else if (elementType.IsSubclassOf(typeof(IntellectObject)))
                    {
                        //Add hot cache.
                        property.CacheProcess = delegate(IMemorySegmentProxy innerProxy, IntellectPropertyAttribute innerAttribute, ToBytesAnalyseResult innerAnalyseResult, object innerTarget, bool innerIsArrayElement, bool innerNullable)
                        {
                            IIntellectObject[] array = innerAnalyseResult.GetValue<IIntellectObject[]>(innerTarget);
                            if (array == null)
                            {
                                if (!innerAttribute.IsRequire) return;
                                throw new PropertyNullValueException(string.Format(ExceptionMessage.EX_PROPERTY_VALUE, innerAttribute.Id, innerAnalyseResult.Property.Name, innerAnalyseResult.Property.PropertyType));
                            }
                            //id(1) + total length(4) + rank(4)
                            innerProxy.WriteByte((byte)innerAttribute.Id);
                            MemoryPosition startPosition = innerProxy.GetPosition();
                            innerProxy.Skip(4U);
                            innerProxy.WriteInt32(array.Length);
                            for (int i = 0; i < array.Length; i++)
                            {
                                IIntellectObject element = array[i];
                                if (element == null) innerProxy.WriteUInt16(0);
                                else
                                {
                                    MemoryPosition innerStartObjPosition = innerProxy.GetPosition();
                                    innerProxy.Skip(Size.UInt16);
                                    ToBytes(element, innerProxy);
                                    MemoryPosition innerEndObjPosition = innerProxy.GetPosition();
                                    innerProxy.WriteBackUInt16(innerStartObjPosition, (ushort)(MemoryPosition.CalcLength(innerProxy.SegmentCount, innerStartObjPosition, innerEndObjPosition) - 2));
                                }
                            }
                            MemoryPosition endPosition = innerProxy.GetPosition();
                            innerProxy.WriteBackInt32(startPosition, MemoryPosition.CalcLength(innerProxy.SegmentCount, startPosition, endPosition) - 4);
                        };
                    }
                    else if (!(elementType == typeof(string)) && elementType.IsSerializable)
                        throw new NotSupportedException(string.Format(ExceptionMessage.EX_NOT_SUPPORTED_VALUE, property.Attribute.Id, property.Property.Name, property.Property.PropertyType));
                    else
                    {
                        intellectTypeProcessor = IntellectTypeProcessorMapping.Instance.GetProcessor(property.Attribute.Id) ??
                                                               IntellectTypeProcessorMapping.Instance.GetProcessor(elementType);
                        if (intellectTypeProcessor == null) throw new NotSupportedException(string.Format(ExceptionMessage.EX_NOT_SUPPORTED_VALUE, property.Attribute.Id, property.Property.Name, elementType));
                        //Add hot cache.
                        IIntellectTypeProcessor processor = intellectTypeProcessor;
                        property.CacheProcess = delegate(IMemorySegmentProxy innerProxy, IntellectPropertyAttribute innerAttribute, ToBytesAnalyseResult innerAnalyseResult, object innerTarget, bool innerIsArrayElement, bool innerNullable)
                        {
                            Array array = innerAnalyseResult.GetValue<Array>(innerTarget);
                            if (array == null)
                            {
                                if (!innerAttribute.IsRequire) return;
                                throw new PropertyNullValueException(string.Format(ExceptionMessage.EX_PROPERTY_VALUE, innerAttribute.Id, innerAnalyseResult.Property.Name, innerAnalyseResult.Property.PropertyType));
                            }
                            //id(1) + total length(4) + rank(4)
                            innerProxy.WriteByte((byte)innerAttribute.Id);
                            MemoryPosition startPosition = innerProxy.GetPosition();
                            innerProxy.Skip(4);
                            innerProxy.WriteInt32(array.Length);
                            for (int i = 0; i < array.Length; i++)
                            {
                                object element = array.GetValue(i);
                                if (element == null) innerProxy.WriteUInt16(0);
                                else
                                {
                                    MemoryPosition innerStartObjPosition = innerProxy.GetPosition();
                                    innerProxy.Skip(Size.UInt16);
                                    processor.Process(innerProxy, innerAttribute, innerAnalyseResult, element, true);
                                    MemoryPosition innerEndObjPosition = innerProxy.GetPosition();
                                    innerProxy.WriteBackUInt16(innerStartObjPosition, (ushort)(MemoryPosition.CalcLength(innerProxy.SegmentCount, innerStartObjPosition, innerEndObjPosition) - 2));
                                }
                            }
                            MemoryPosition endPosition = innerProxy.GetPosition();
                            innerProxy.WriteBackInt32(startPosition, MemoryPosition.CalcLength(innerProxy.SegmentCount, startPosition, endPosition) - 4);
                        };
                    }
                    property.CacheProcess(proxy, property.Attribute, property, obj, false, false);
                    property.HasCacheFinished = true;
                    continue;
                }

                #endregion

                throw new NotSupportedException(string.Format(ExceptionMessage.EX_NOT_SUPPORTED_VALUE, property.Attribute.Id, property.Property.Name, property.Property.PropertyType));
            }
            MemoryPosition wrapperEndPosition = proxy.GetPosition();
            proxy.WriteBackInt32(wrapperStartPosition,MemoryPosition.CalcLength(proxy.SegmentCount, wrapperStartPosition, wrapperEndPosition));
        }

        /// <summary>
        ///     ��һ��Ԫ����ת��Ϊ�ض����͵Ķ���
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="data">Ԫ����</param>
        /// <returns>����ת������ض�����</returns>
        /// <exception cref="Exception">ת��ʧ��</exception>
        public static T GetObject<T>(byte[] data)
        {
            return GetObject<T>(typeof(T), data, 0, data.Length);
        }

        /// <summary>
        ///     ��һ��Ԫ����ת��Ϊ�ض����͵Ķ���
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="target">�ض��Ķ���</param>
        /// <param name="data">Ԫ����</param>
        /// <returns>����ת������ض�����</returns>
        /// <exception cref="Exception">ת��ʧ��</exception>
        public static T GetObject<T>(Type target, byte[] data)
        {
            return GetObject<T>(target, data, 0, data.Length);
        }

        /// <summary>
        ///     ��һ��Ԫ����ת��Ϊ�ض����͵Ķ���
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="target">�ض��Ķ���</param>
        /// <param name="data">Ԫ����</param>
        /// <param name="offset">Ԫ����ƫ��</param>
        /// <param name="length">����Ԫ���ݳ���</param>
        /// <returns>����ת������ض�����</returns>
        /// <exception cref="Exception">ת��ʧ��</exception>
        public static T GetObject<T>(Type target, byte[] data, int offset, int length)
        {
            if (target == null) throw new ArgumentNullException("target");
            if (data == null) throw new ArgumentNullException("data");
            Dictionary<int, GetObjectAnalyseResult> result = Analyser.GetObjectAnalyser.Analyse(target);
            if (result == null) return default(T);
            #region �������ݲ���

            int id, currentLength, chunkSize;
            chunkSize = offset + length;
            int innerOffset = offset + 4;
            int totalLength = BitConverter.ToInt32(data, offset);
            if (totalLength != length) throw new System.Exception("Illegal binary data length! #length: " + totalLength);
            //create instance for new obj.
            Object instance = Activator.CreateInstance(target);
            IntellectObject intellectObject = instance as IntellectObject;
            if (intellectObject == null) throw new System.Exception("Cannot convert target object to Intellect Object! #type: " + target.FullName);
            do
            {
                //get id.
                id = data[innerOffset++];
                //get analyze result.
                GetObjectAnalyseResult analyzeResult;
                if (!result.TryGetValue(id, out analyzeResult))
                {
                    if(!MemoryAllotter.AllowCompatibleMode) throw new System.Exception("Illegal data contract, non-exists id! #id: " + id);
                    intellectObject.CompatibleMode = true;
                    intellectObject.IsPickup = true;
                    return (T)instance;
                }
                //calc data length.
                if (analyzeResult.VT) currentLength = analyzeResult.VTStruct.Size;
                else
                {
                    currentLength = BitConverter.ToInt32(data, innerOffset);
                    innerOffset += 4;
                }
                //set current property value to the target object.
                InstanceHelper.SetInstance(intellectObject, analyzeResult, data, innerOffset, currentLength);
                innerOffset += currentLength;
            } while (innerOffset < data.Length && innerOffset < chunkSize);

            #endregion
            intellectObject.IsPickup = true;
            return (T)instance;
        }

        #endregion
    }
}