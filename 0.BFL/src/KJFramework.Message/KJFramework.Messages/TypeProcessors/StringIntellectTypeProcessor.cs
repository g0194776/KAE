using System;
using System.Text;
using KJFramework.Messages.Analysers;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Exceptions;
using KJFramework.Messages.Proxies;

namespace KJFramework.Messages.TypeProcessors
{
    /// <summary>
    ///    String类型智能处理器，提供了相关的基本操作。
    /// </summary>
    public class StringIntellectTypeProcessor : IntellectTypeProcessor
    {
        #region 构造函数

        /// <summary>
        ///     String类型智能处理器，提供了相关的基本操作。
        /// </summary>
        public StringIntellectTypeProcessor()
        {
            _supportedType = typeof(string);
        }

        #endregion

        #region Overrides of IntellectTypeProcessor

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
            string value = isArrayElement ? (string)target : analyseResult.GetValue<string>(target);
            if (value == null)
            {
                if (!attribute.IsRequire || isArrayElement) return;
                throw new PropertyNullValueException(string.Format(ExceptionMessage.EX_PROPERTY_VALUE, attribute.Id, analyseResult.Property.Name, analyseResult.Property.PropertyType));
            }
            if (!isArrayElement)
            {
                proxy.WriteByte((byte)attribute.Id);
                proxy.WriteInt32(Encoding.UTF8.GetByteCount(value));
            }
            proxy.WriteString(value);
        }

        /// <summary>
        ///     从第三方客户数据转换为元数据
        ///     <para>* 此方法将会被轻量级的DataHelper所使用，并且写入的数据将不会拥有编号(Id)</para>
        /// </summary>
        /// <param name="proxy">内存片段代理器</param>
        /// <param name="target">目标对象实例</param>
        /// <param name="isArrayElement">当前写入的值是否为数组元素标示</param>
        /// <param name="isNullable">是否为可空字段标示</param>
        public override void Process(IMemorySegmentProxy proxy, object target, bool isArrayElement = false, bool isNullable = false)
        {
            if (string.IsNullOrEmpty((string) target)) return;
            string value = (string) target;
            proxy.WriteString(value);
        }

        /// <summary>
        ///     从元数据转换为第三方客户数据
        /// </summary>
        /// <param name="attribute">当前字段标注的属性</param>
        /// <param name="data">元数据</param>
        /// <returns>返回转换后的第三方客户数据</returns>
        /// <exception cref="Exception">转换失败</exception>
        public override object Process(IntellectPropertyAttribute attribute, byte[] data)
        {
            if (attribute == null) throw new ArgumentNullException("attribute");
            if (attribute.IsRequire && data == null)
                throw new System.Exception("Cannot process a required value, because current binary data is null! #attr id: " + attribute.Id);
            if (data == null) return null;
            if (data.Length == 0) return "";
            return Encoding.UTF8.GetString(data);
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
            if (length == 0 || data == null || data.Length == 0) return;
            unsafe
            {
                fixed (byte* old = &data[offset])
                {
                    int charCount = Encoding.UTF8.GetCharCount(old, length);
                    //allcate memory at thread stack.
                    if (charCount <= MemoryAllotter.CharSizeCanAllcateAtStack)
                    {
                        char* newObj = stackalloc char[charCount];
                        int len = Encoding.UTF8.GetChars(old, length, newObj, charCount);
                        result.SetValue(instance, new string(newObj, 0, len));
                    }
                    //allocate memory at heap.
                    else
                    {
                        fixed (char* newObj = new char[charCount])
                        {
                            int len = Encoding.UTF8.GetChars(old, length, newObj, charCount);
                            result.SetValue(instance, new string(newObj, 0, len));
                        }
                    }
                }
            }
        }

        #endregion
    }
}