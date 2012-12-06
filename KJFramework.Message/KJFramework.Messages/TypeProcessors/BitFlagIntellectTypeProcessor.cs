using System;
using KJFramework.Messages.Analysers;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Helpers;
using KJFramework.Messages.Proxies;
using KJFramework.Messages.Types;

namespace KJFramework.Messages.TypeProcessors
{
    /// <summary>
    ///     BitFlag位标示类型处理器
    /// </summary>
    public class BitFlagIntellectTypeProcessor : IntellectTypeProcessor
    {
        #region Constructor

        /// <summary>
        ///     智能的类型处理器抽象父类，提供了相关的基本操作。
        /// </summary>
        public BitFlagIntellectTypeProcessor()
        {
            _supportedType = typeof(BitFlag);
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
        public override void Process(byte[] memory, int offset, IntellectPropertyAttribute attribute, object value)
        {
            memory[offset] = BitConvertHelper.ConvertToByte((BitFlag)value);
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
            BitFlag value = analyseResult.GetValue<BitFlag>(target);
            proxy.WriteByte((byte)attribute.Id);
            proxy.WriteBitFlag(value);
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
            if (attribute.IsRequire)
            {
                if (value == null) throw new ArgumentNullException("value");
                return new[] { BitConvertHelper.ConvertToByte((BitFlag)value) };
            }
            return value == null ? null : new[] { BitConvertHelper.ConvertToByte((BitFlag)value) };
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
            if (attribute.IsRequire)
            {
                if (data == null) throw new ArgumentNullException("data");
                return new BitFlag(data[0]);
            }
            return data == null ? null : new BitFlag(data[0]);
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
            if (attribute.IsRequire)
            {
                if (data == null) throw new ArgumentNullException("data");
                return new BitFlag(data[offset]);
            }
            return data == null ? null : new BitFlag(data[offset]);
        }

        /// <summary>
        ///     从元数据转换为第三方客户数据
        /// </summary>
        /// <param name="instance">目标对象</param>
        /// <param name="result">分析结果</param>
        /// <param name="data">元数据</param>
        /// <param name="offset">元数据所在的便宜量</param>
        /// <param name="length">元数据长度</param>
        public override void Process(object instance, GetObjectAnalyseResult result, byte[] data, int offset, int length = 0)
        {
            result.SetValue(instance, new BitFlag(data[offset]));
        }

        #endregion
    }
}