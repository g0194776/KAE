using System;
using System.Collections.Generic;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Engine;
using KJFramework.Messages.Helpers;
using KJFramework.Messages.Objects;
using KJFramework.Messages.TypeProcessors.Maps;

namespace KJFramework.Messages.TypeProcessors
{
    /// <summary>
    ///    Array类型智能处理器，提供了相关的基本操作。
    /// </summary>
    public class NormalArrayIntellectTypeProcessor : IntellectTypeProcessor
    {
        #region 构造函数

        /// <summary>
        ///     Array类型智能处理器，提供了相关的基本操作。
        /// </summary>
        public NormalArrayIntellectTypeProcessor()
        {
            _supportedType = typeof (Array);
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
        /// <param name="attribute">当前字段标注的属性</param>
        /// <param name="value">第三方客户数据</param>
        /// <returns>返回转换后的元数据</returns>
        /// <exception cref="Exception">转换失败</exception>
        public override byte[] Process(IntellectPropertyAttribute attribute, object value)
        {
            if (attribute == null) throw new ArgumentNullException("attribute");
            if (attribute.IsRequire && value == null) throw new System.Exception("Cannot process illegal type value! #attr id: " + attribute.Id);
            Array array = (Array)value;
            Type type = value.GetType().GetElementType();
            VT vt = FixedTypeManager.IsVT(type);
            Func<IntellectPropertyAttribute, Type, Object, byte[]> function;
            IIntellectTypeProcessor processor = null;
            if (type.IsSubclassOf(typeof(IntellectObject))) function = ToBytesWithIntellectObject;
            else if (vt != null)
            {
                processor = IntellectTypeProcessorMapping.Instance.GetProcessor(attribute.Id) ??
                                    IntellectTypeProcessorMapping.Instance.GetProcessor(type);
                if (processor == null) throw new System.Exception("Cannot support this data type processor! #type: " + type);
                function = ToBytesWithNormalTypeObject;
            }
            else if (!(type == typeof(string)) && type.IsSerializable) function = ToBytesWithSerlizeObject;
            else function = ToBytesWithNormalTypeObject;
            

            #region 提取数组中的每个元素

            List<byte[]> datas;
            int totalLength = 4;
            if(vt == null)
            {
                datas = new List<byte[]>();
                for (int i = 0; i < array.Length; i++)
                {
                    Object current = array.GetValue(i);
                    byte[] temp = IntellectObjectHelper.SetLength(function(attribute, type, current));
                    if (temp == null) throw new System.Exception("Cannot convert data from array elements! #type: " + type);
                    totalLength += temp.Length;
                    datas.Add(temp);
                }
            }
            //fixed binary length for each element.
            else
            {
                #region Deal VT type array specal.

                totalLength += array.Length * vt.Size;
                byte[] finalData = new byte[totalLength + 5];
                finalData[0] = (byte)attribute.Id;
                //total length.
                BitConvertHelper.GetBytes(totalLength, finalData, 1);
                //rank.
                BitConvertHelper.GetBytes(array.Length, finalData, 5);
                int innerVtOffset = 9;
                for (int i = 0; i < array.Length; i++)
                {
                    Object current = array.GetValue(i);
                    processor.Process(finalData, innerVtOffset, attribute, current);
                    innerVtOffset += vt.Size;
                }
                return finalData;

                #endregion
            }

            #endregion

            #region 拼接数据

            byte[] realData = new byte[totalLength + 5];
            realData[0] = (byte)attribute.Id;
            BitConvertHelper.GetBytes(totalLength, realData, 1);
            //rank.
            BitConvertHelper.GetBytes(array.Length, realData, 5);
            int offset = 9;
            foreach (byte[] bytes in datas)
            {
                Buffer.BlockCopy(bytes, 0, realData, offset, bytes.Length);
                offset += bytes.Length;
            }
            return realData;

	        #endregion
        }

        /// <summary>
        ///     从元数据转换为第三方客户数据
        /// </summary>
        /// <param name="attribute">当前字段标注的属性</param>
        /// <param name="data">元数据</param>
        /// <returns>返回转换后的第三方客户数据</returns>
        /// <exception cref="Exception">转换失败</exception>
        [Obsolete("当前类型处理器不支持此操作。", true)]
        public override object Process(IntellectPropertyAttribute attribute, byte[] data)
        {
            return null;
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
        [Obsolete("当前类型处理器不支持此操作。", true)]
        public override object Process(IntellectPropertyAttribute attribute, byte[] data, int offset, int length = 0)
        {
            return null;
        }

        /// <summary>
        ///     从元数据转换为第三方客户数据
        /// </summary>
        /// <param name="attribute">当前字段标注的属性</param>
        /// <param name="type">元素类型</param>
        /// <param name="data">元数据</param>
        /// <returns>返回转换后的第三方客户数据</returns>
        /// <exception cref="Exception">转换失败</exception>
        public object Process(IntellectPropertyAttribute attribute, Type type, byte[] data)
        {
            return Process(attribute, type, data, 0, data.Length);
        }

        /// <summary>
        ///     从元数据转换为第三方客户数据
        /// </summary>
        /// <param name="attribute">当前字段标注的属性</param>
        /// <param name="type">元素类型</param>
        /// <param name="data">元数据</param>
        /// <param name="offset">元数据所在的偏移量</param>
        /// <param name="length">元数据长度</param>
        /// <returns>返回转换后的第三方客户数据</returns>
        /// <exception cref="Exception">转换失败</exception>
        public object Process(IntellectPropertyAttribute attribute, Type type, byte[] data, int offset, int length = 0)
        {
            if (attribute == null) throw new ArgumentNullException("attribute");
            if (attribute.IsRequire && (data == null))
                throw new System.Exception("Cannot process a required value, because current binary data is null! #attr id: " + attribute.Id);
            if (data == null) return null;
            Func<IntellectPropertyAttribute, Type, byte[], int, int, object> function;
            VT vt = FixedTypeManager.IsVT(type);
            if (vt != null) function = GetObjectWithNormalTypeObject;
            else if (type.IsSubclassOf(typeof(IntellectObject))) function = GetObjectWithIntellectObject;
            else if (!(type == typeof(string)) && type.IsSerializable) function = GetObjectWithSerlizeObject;
            else function = GetObjectWithNormalTypeObject;
            //create new instance for a array.
            Array ins = Array.CreateInstance(type, BitConverter.ToInt32(data, offset));
            //process empty array.
            if (ins.Length == 0) return ins;
            int elementOffset = 0;
            int chunkSize = offset + length;
            //real data + 4 offset of array length.
            int innerOffset = offset + 4;
            if (vt == null)
            {
                #region Process dynamic data type.

                do
                {
                    short currentLength = BitConverter.ToInt16(data, innerOffset);
                    innerOffset += 2;
                    if (currentLength == 0) ins.SetValue(null, elementOffset);
                    else
                    {
                        if ((data.Length - innerOffset) < currentLength)
                            throw new System.Exception("Illegal remaining binary data length!");
                        ins.SetValue(function(attribute, type, data, innerOffset, currentLength), elementOffset);
                        innerOffset += currentLength;
                    }
                    elementOffset++;
                } while (innerOffset < data.Length && innerOffset < chunkSize);

                #endregion
            }
            else
            {
                #region Process fixed data type.

                do
                {
                    if ((data.Length - innerOffset) < vt.Size)
                        throw new System.Exception("Illegal remaining binary data length!");
                    ins.SetValue(function(attribute, type, data, innerOffset, vt.Size), elementOffset);
                    innerOffset += vt.Size;
                    elementOffset++;
                } while (innerOffset < data.Length && innerOffset < chunkSize);

                #endregion
            }
            return ins;
        }

        #endregion

        #region Methods

        private byte[] ToBytesWithIntellectObject(IntellectPropertyAttribute attribute, Type type, Object value)
        {
            byte[] data = IntellectObjectEngine.ToBytes((IIntellectObject)value);
            return data;
        }

        private Object GetObjectWithIntellectObject(IntellectPropertyAttribute attribute, Type type, byte[] data, int offset, int length)
        {
            if (attribute.IsRequire && data == null)
                throw new System.Exception("Cannot process a required value, because current binary data is null! #attr id: " + attribute.Id);
            if (data == null) return null;
            Object instance = IntellectObjectEngine.GetObject<Object>(type, data, offset, length);
            if (instance == null) throw new System.Exception("Cannot convert binary data to a Intellect Object. #attr id: " + attribute.Id);
            return instance;
        }

        private byte[] ToBytesWithSerlizeObject(IntellectPropertyAttribute attribute, Type type, Object value)
        {
            IIntellectTypeProcessor intellectTypeProcessor = IntellectTypeProcessorMapping.Instance.GetProcessor(typeof(ClassSerializeObject));
            return intellectTypeProcessor.Process(attribute, value);
        }

        private Object GetObjectWithSerlizeObject(IntellectPropertyAttribute attribute, Type type, byte[] data, int offset, int length)
        {
            if (attribute.IsRequire && data == null)
                throw new System.Exception("Cannot process a required value, because current binary data is null! #attr id: " + attribute.Id);
            if (data == null) return null;
            IIntellectTypeProcessor intellectTypeProcessor = IntellectTypeProcessorMapping.Instance.GetProcessor(typeof(ClassSerializeObject));
            return intellectTypeProcessor.Process(attribute, data);
        }

        private byte[] ToBytesWithNormalTypeObject(IntellectPropertyAttribute attribute, Type type, Object value)
        {
            IIntellectTypeProcessor intellectTypeProcessor = IntellectTypeProcessorMapping.Instance.GetProcessor(attribute.Id) ??
                                                                                    IntellectTypeProcessorMapping.Instance.GetProcessor(type);
            if (intellectTypeProcessor == null) throw new System.Exception("Cannot support this data type processor! #type: " + type);
            return intellectTypeProcessor.Process(attribute, value);
        }

        private object GetObjectWithNormalTypeObject(IntellectPropertyAttribute attribute, Type type, byte[] data, int offset, int length)
        {
            if (attribute.IsRequire && (data == null))
                throw new System.Exception("Cannot process a required value, because current binary data is null! #attr id: " + attribute.Id);
            if (data == null) return null;
            IIntellectTypeProcessor intellectTypeProcessor = IntellectTypeProcessorMapping.Instance.GetProcessor(attribute.Id) ??
                                                                                    IntellectTypeProcessorMapping.Instance.GetProcessor(type);
            if (intellectTypeProcessor == null) throw new System.Exception("Cannot support this data type processor! #type: " + type);
            return intellectTypeProcessor.SupportUnmanagement
           ? intellectTypeProcessor.Process(attribute, data, offset, length)
           : intellectTypeProcessor.Process(attribute, data);
        }
 
        #endregion
    }
}