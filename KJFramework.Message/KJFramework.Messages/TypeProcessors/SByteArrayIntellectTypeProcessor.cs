using System;
using KJFramework.Core.Native;
using KJFramework.Messages.Analysers;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Exceptions;
using KJFramework.Messages.Helpers;
using KJFramework.Messages.Proxies;

namespace KJFramework.Messages.TypeProcessors
{
    /// <summary>
    ///     SByte数组类型处理器
    /// </summary>
    public class SByteArrayIntellectTypeProcessor : IntellectTypeProcessor
    {
        #region Constructor

        /// <summary>
        ///     SByte数组类型处理器
        /// </summary>
        public SByteArrayIntellectTypeProcessor()
        {
            _supportedType = typeof(sbyte[]);
            _supportUnmanagement = true;
        }

        #endregion

        #region Overrides of IntellectTypeProcessor

        /// <summary>
        ///     从第三方客户数据转换为元数据
        /// </summary>
        /// <param name="memory">需要填充的字节数组</param>
        /// <param name="offset">需要填充数组的起始偏移量</param>
        /// <param name="attribute">当前字段标注的属性</param>
        /// <param name="value">第三方客户数据</param>
        [Obsolete("Cannot use this method, because current type doesn't supported.", true)]
        public override void Process(byte[] memory, int offset, IntellectPropertyAttribute attribute, object value)
        {
            throw new NotSupportedException("Cannot use this method, because current type doesn't supported.");
        }

        /// <summary>
        ///     从第三方客户数据转换为元数据
        /// </summary>
        /// <param name="proxy">内存片段代理器</param>
        /// <param name="attribute">字段属性</param>
        /// <param name="analyseResult">分析结果</param>
        /// <param name="target">目标对象实例</param>
        /// <param name="isArrayElement">当前写入的值是否为数组元素标示</param>
        /// <param name="isNullable">是否为可空字段标示</param>
        public override void Process(IMemorySegmentProxy proxy, IntellectPropertyAttribute attribute, ToBytesAnalyseResult analyseResult, object target, bool isArrayElement = false, bool isNullable = false)
        {
            sbyte[] value = analyseResult.GetValue<sbyte[]>(target);
            if (value == null)
            {
                if (!attribute.IsRequire) return;
                throw new PropertyNullValueException(string.Format(ExceptionMessage.EX_PROPERTY_VALUE, attribute.Id, analyseResult.Property.Name, analyseResult.Property.PropertyType));
            }
            //id(1) + total length(4) + rank(4)
            proxy.WriteByte((byte)attribute.Id);
            MemoryPosition position = proxy.GetPosition();
            proxy.Skip(4U);
            proxy.WriteInt32(value.Length);
            if (value.Length == 0)
            {
                proxy.WriteBackInt32(position, 4);
                return;
            }
            if (value.Length > 10)
            {
                unsafe
                {
                    fixed (sbyte* pByte = value) proxy.WriteMemory(pByte, (uint) value.Length*Size.SByte);
                }
            }
            else for (int i = 0; i < value.Length; i++) proxy.WriteSByte(value[i]);
            proxy.WriteBackInt32(position, (int) (value.Length*Size.SByte + 4));
        }

        /// <summary>
        ///     从第三方客户数据转换为元数据
        /// </summary>
        /// <param name="attribute">当前字段标注的属性</param>
        /// <param name="value">第三方客户数据</param>
        /// <returns>返回转换后的元数据</returns>
        /// <exception cref="Exception">转换失败</exception>
        public override byte[] Process(IntellectPropertyAttribute attribute, object value)
        {
            byte[] memory;
            if (value == null && attribute.IsRequire) throw new ArgumentNullException("value");
            sbyte[] arr = (sbyte[])value;
            //id(1) + total length(4) + rank(4)
            memory = new byte[9 + Size.SByte*arr.Length];
            memory[0] = (byte)attribute.Id;
            BitConvertHelper.GetBytes(memory.Length - 5, memory, 1);
            BitConvertHelper.GetBytes(arr.Length, memory, 5);
            if (arr.Length == 0) return memory;
            int offset = 9;
            unsafe
            {
                fixed (sbyte* pInt = arr)
                {
                    fixed (byte* pData = &memory[offset])
                    {
                        sbyte* pTemp = pInt;
                        sbyte* pArray = (sbyte*)pData;
                        for (int i = 0; i < arr.Length; i++)
                            *(pArray++) = *(pTemp++);
                    }
                }
            }
            return memory;
        }

        /// <summary>
        ///     从元数据转换为第三方客户数据
        /// </summary>
        /// <param name="attribute">当前字段标注的属性</param>
        /// <param name="data">元数据</param>
        /// <returns>返回转换后的第三方客户数据</returns>
        /// <exception cref="Exception">转换失败</exception>
        [Obsolete("Cannot use this method, because current type doesn't supported.", true)]
        public override object Process(IntellectPropertyAttribute attribute, byte[] data)
        {
            throw new NotSupportedException("Cannot use this method, because current type doesn't supported.");
        }

        /// <summary>
        ///     从元数据转换为第三方客户数据
        /// </summary>
        /// <param name="attribute">当前字段标注的属性</param>
        /// <param name="data">元数据</param>
        /// <param name="offset">元数据所在的偏移量</param>
        /// <param name="length">元数据长度</param>
        /// <returns>返回转换后的第三方客户数据</returns>
        /// <exception cref="Exception">转换失败</exception>
        public override object Process(IntellectPropertyAttribute attribute, byte[] data, int offset, int length = 0)
        {
            sbyte[] ret;
            if (length == 4) return new sbyte[0];
            unsafe
            {
                fixed (byte* pByte = &data[offset])
                {
                    int arrLength = *(int*)pByte;
                    sbyte* pTemp = (sbyte*)(pByte + 4);
                    ret = new sbyte[arrLength];
                    for (int i = 0; i < arrLength; i++)
                        ret[i] = *(pTemp++);
                }
            }
            return ret;
        }

        /// <summary>
        ///     从元数据转换为第三方客户数据
        /// </summary>
        /// <param name="instance">目标对象</param>
        /// <param name="result">分析结果</param>
        /// <param name="data">元数据</param>
        /// <param name="offset">元数据所在的偏移量</param>
        /// <param name="length">元数据长度</param>
        public override void Process(object instance, GetObjectAnalyseResult result, byte[] data, int offset, int length = 0)
        {
            if (length == 4)
            {
                result.SetValue(instance, new sbyte[0]);
                return;
            }
            sbyte[] array;
            unsafe
            {
                fixed (byte* pByte = &data[offset])
                {
                    int arrLength = *(int*)pByte;
                    array = new sbyte[arrLength];
                    if (arrLength > 10)
                    {
                        fixed (sbyte* point = array)
                        {
                            Native.Win32API.memcpy(new IntPtr((byte*)point), new IntPtr(pByte + 4), (uint)(Size.SByte * arrLength));
                        }

                    }
                    else
                    {
                        sbyte* point = (sbyte*)(pByte + 4);
                        for (int i = 0; i < arrLength; i++)
                            array[i] = *(point++);
                    }
                }
            }
            result.SetValue(instance, array);
        }

        #endregion
    }
}