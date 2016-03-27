using System;
using KJFramework.Messages.Analysers;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Objects;
using KJFramework.Messages.Proxies;
using KJFramework.Serializers;

namespace KJFramework.Messages.TypeProcessors
{
    /// <summary>
    ///    Class Serialize类型智能处理器，提供了相关的基本操作。
    /// </summary>
    public class ClassSerializeIntellectTypeProcessor : IntellectTypeProcessor
    {
        #region 构造函数

        /// <summary>
        ///     Class Serialize类型智能处理器，提供了相关的基本操作。
        /// </summary>
        public ClassSerializeIntellectTypeProcessor()
        {
            _supportedType = typeof(ClassSerializeObject);
            _supportUnmanagement = false;
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
        [Obsolete("Cannot use this method, because current type doesn't supported.", true)]
        public override void Process(IMemorySegmentProxy proxy, IntellectPropertyAttribute attribute, ToBytesAnalyseResult analyseResult, object target, bool isArrayElement = false, bool isNullable = false)
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
            if (attribute == null) throw new System.Exception("非法的智能属性标签。");
            if (attribute.IsRequire && value == null) throw new System.Exception("无法处理非法的类型值。");
            if (value == null) return null;
            using (ClassMetadataSerializer classMetadataSerializer = new ClassMetadataSerializer())
            {
                byte[] data = classMetadataSerializer.Serialize(value);
                if (attribute.IsRequire && data == null)
                {
                    throw new System.Exception("无法处理非法的值。");
                }
                return data;
            }
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
            if (attribute == null) throw new System.Exception("非法的智能属性标签。");
            if (attribute.IsRequire && data == null) throw new System.Exception("无法处理非法的类型值。");
            if (data != null)
            {
                using (ClassMetadataSerializer classMetadataSerializer = new ClassMetadataSerializer())
                {
                    object value = classMetadataSerializer.Deserialize<Object>(data);
                    if (attribute.IsRequire && value == null)
                    {
                        throw new System.Exception("无法处理非法的值。");
                    }
                    return value;
                }
            }
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
        public override object Process(IntellectPropertyAttribute attribute, byte[] data, int offset, int length = 0)
        {
            throw new NotImplementedException("Pls use management method to un-serialize current data!");
        }

        #endregion
    }
}