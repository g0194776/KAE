using System;
using System.Text;
using KJFramework.Messages.Analysers;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Exceptions;
using KJFramework.Messages.Proxies;

namespace KJFramework.Messages.TypeProcessors
{
    /// <summary>
    ///     String数组类型处理器
    /// </summary>
    public class StringArrayIntellectTypeProcessor : IntellectTypeProcessor
    {
        #region Constructor

        /// <summary>
        ///     String数组类型处理器
        /// </summary>
        public StringArrayIntellectTypeProcessor()
        {
            _supportedType = typeof(string[]);
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
        public override void Process(IMemorySegmentProxy proxy, IntellectPropertyAttribute attribute, ToBytesAnalyseResult analyseResult, object target, bool isArrayElement = false, bool isNullable = false)
        {
            string[] value = analyseResult.GetValue<string[]>(target);
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
            for (int i = 0; i < value.Length; i++)
            {
                string elementValue = value[i];
                if (string.IsNullOrEmpty(elementValue)) proxy.WriteUInt16(0);
                else
                {
                    byte[] elementData = Encoding.UTF8.GetBytes(elementValue);
                    proxy.WriteUInt16((ushort)elementData.Length);
                    proxy.WriteMemory(elementData, 0U, (uint)elementData.Length);
                }
            }
            MemoryPosition endPosition = proxy.GetPosition();
            proxy.WriteBackInt32(position, MemoryPosition.CalcLength(proxy.SegmentCount, position, endPosition) - 4);
        }

        /// <summary>
        ///     从第三方客户数据转换为元数据
        /// </summary>
        /// <param name="attribute">当前字段标注的属性</param>
        /// <param name="value">第三方客户数据</param>
        /// <returns>返回转换后的元数据</returns>
        /// <exception cref="Exception">转换失败</exception>
        [Obsolete("Cannot use this method, because current type doesn't supported.", true)]
        public override byte[] Process(IntellectPropertyAttribute attribute, object value)
        {
            throw new NotSupportedException("Cannot use this method, because current type doesn't supported.");
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
        [Obsolete("Cannot use this method, because current type doesn't supported.", true)]
        public override object Process(IntellectPropertyAttribute attribute, byte[] data, int offset, int length = 0)
        {
            throw new NotSupportedException("Cannot use this method, because current type doesn't supported.");
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
            int innerOffset = offset;
            int chunkSize = offset + length;
            int arrLen = BitConverter.ToInt32(data, innerOffset);
            if (arrLen == 0)
            {
                result.SetValue(instance, new string[0]);
                return;
            }
            string[] strArr = new string[arrLen];
            innerOffset += 4;
            int arrIndex = 0;
            short size;
            do
            {
                size = BitConverter.ToInt16(data, innerOffset);
                innerOffset += 2;
                if ((data.Length - innerOffset) < size)
                    throw new System.Exception("Illegal remaining binary data length!");
                //use unmanagement method by default.
                if (size == 0)
                {
                    strArr[arrIndex] = null;
                }
                else
                {
                    string str;
                    unsafe
                    {
                        fixed (byte* old = &data[innerOffset])
                        {
                            int charCount = Encoding.UTF8.GetCharCount(old, size);
                            //allcate memory at thread stack.
                            if(charCount <= MemoryAllotter.CharSizeCanAllcateAtStack)
                            {
                                char* newObj = stackalloc char[charCount];
                                int len = Encoding.UTF8.GetChars(old, size, newObj, charCount);
                                str = new string(newObj, 0, len);
                            }
                            //allocate memory at heap.
                            else
                            {
                                fixed (char* newObj = new char[charCount])
                                {
                                    int len = Encoding.UTF8.GetChars(old, size, newObj, charCount);
                                    str = new string(newObj, 0, len);
                                }
                            }
                        }
                    }
                    strArr[arrIndex] = str;
                }
                innerOffset += size;
                arrIndex++;
            } while (innerOffset < data.Length && innerOffset < chunkSize);
            result.SetValue(instance, strArr);
        }

        #endregion
    }
}