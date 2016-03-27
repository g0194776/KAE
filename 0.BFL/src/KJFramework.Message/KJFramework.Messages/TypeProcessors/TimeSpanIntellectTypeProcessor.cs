using System;
using KJFramework.Messages.Analysers;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Exceptions;
using KJFramework.Messages.Helpers;
using KJFramework.Messages.Proxies;

namespace KJFramework.Messages.TypeProcessors
{
    /// <summary>
    ///     TimeSpan结构类型处理器
    /// </summary>
    public class TimeSpanIntellectTypeProcessor : IntellectTypeProcessor
    {
        #region Constructor

        /// <summary>
        ///     TimeSpan结构类型处理器
        /// </summary>
        public TimeSpanIntellectTypeProcessor()
        {
            _supportUnmanagement = true;
            _supportedType = typeof(TimeSpan);
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
            TimeSpan value;
            if (!isNullable) value = analyseResult.GetValue<TimeSpan>(target);
            else
            {
                TimeSpan? nullableValue = analyseResult.GetValue<TimeSpan?>(target);
                if (nullableValue == null)
                {
                    if (!attribute.IsRequire) return;
                    throw new PropertyNullValueException(
                        string.Format(ExceptionMessage.EX_PROPERTY_VALUE, attribute.Id,
                                        analyseResult.Property.Name,
                                        analyseResult.Property.PropertyType));
                }
                value = (TimeSpan)nullableValue;
            }
            if (attribute.AllowDefaultNull && value.Equals(DefaultValue.TimeSpan) && !isArrayElement) return;
            proxy.WriteByte((byte)attribute.Id);
            proxy.WriteInt64(value.Ticks);
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
            TimeSpan value;
            if (!isNullable) value = (TimeSpan)target;
            else
            {
                if (target == null) return;
                value = (TimeSpan)target;
            }
            proxy.WriteTimeSpan(value);
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
            unsafe
            {
                fixed (byte* pData = data)
                    return new TimeSpan(*(long*)pData);
            }
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
            unsafe
            {
                fixed (byte* pData = &data[offset])
                {
                    if (result.Nullable) result.SetValue<TimeSpan?>(instance, new TimeSpan(*(long*)pData));
                    else result.SetValue(instance, new TimeSpan(*(long*)pData));
                }
            }
        }

        #endregion
    }
}