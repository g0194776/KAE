using System;
using KJFramework.Messages.Analysers;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Exceptions;
using KJFramework.Messages.Helpers;
using KJFramework.Messages.Proxies;

namespace KJFramework.Messages.TypeProcessors
{
    /// <summary>
    ///     UInt64类型智能处理器，提供了相关的基本操作。
    /// </summary>
    public class UInt64IntellectTypeProcessor : IntellectTypeProcessor
    {
        #region 构造函数

        /// <summary>
        ///     UInt64类型智能处理器，提供了相关的基本操作。
        /// </summary>
        public UInt64IntellectTypeProcessor()
        {
            _supportedType = typeof (ulong);
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
            BitConvertHelper.GetBytes((long)(ulong)value, memory, offset);
        }

        /// <summary>
        ///     从第三方客户数据转换为元数据
        /// </summary>
        /// <param name="proxy">内存片段代理器</param>
        /// <param name="attribute">字段属性</param>
        /// <param name="analyseResult">分析结果</param>
        /// <param name="target">目标对象实例</param>
        /// <param name="isArrayElement">当前写入的值是否为数组元素标示</param>
        public unsafe override void Process(IMemorySegmentProxy proxy, IntellectPropertyAttribute attribute, ToBytesAnalyseResult analyseResult, object target, bool isArrayElement = false, bool isNullable = false)
        {
            ulong value;
            if (!isNullable) value = analyseResult.GetValue<ulong>(target);
            else
            {
                ulong? nullableValue = analyseResult.GetValue<ulong?>(target);
                if (nullableValue == null)
                {
                    if (!attribute.IsRequire) return;
                    throw new PropertyNullValueException(
                        string.Format(ExceptionMessage.EX_PROPERTY_VALUE, attribute.Id,
                                        analyseResult.Property.Name,
                                        analyseResult.Property.PropertyType));
                }
                value = (ulong)nullableValue;
            }
            if (attribute.AllowDefaultNull && value.Equals(DefaultValue.UInt64) && !isArrayElement) return;
            proxy.WriteByte((byte) attribute.Id);
            proxy.WriteUInt64(&value);
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
            return BitConverter.GetBytes((ulong) value);
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
            return Process(attribute, data, 0);
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
            if (attribute == null) throw new System.Exception("非法的智能属性标签。");
            if (attribute.IsRequire && data == null) throw new System.Exception("无法处理非法的类型值。");
            return BitConverter.ToUInt64(data, offset);
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
           if(result.Nullable) result.SetValue<ulong?>(instance, BitConverter.ToUInt64(data, offset));
           else result.SetValue(instance, BitConverter.ToUInt64(data, offset));
        }

        #endregion
    }
}