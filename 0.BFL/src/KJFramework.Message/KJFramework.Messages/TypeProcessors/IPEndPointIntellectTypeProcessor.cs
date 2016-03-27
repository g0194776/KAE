using System.Net;
using KJFramework.Messages.Analysers;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Exceptions;
using KJFramework.Messages.Proxies;

namespace KJFramework.Messages.TypeProcessors
{
    /// <summary>
    ///     IPEndPoint类型处理器
    /// </summary>
    public class IPEndPointIntellectTypeProcessor : IntellectTypeProcessor
    {
        #region Constructor

        /// <summary>
        ///     IPEndPoint类型处理器
        /// </summary>
        public IPEndPointIntellectTypeProcessor()
        {
            _supportedType = typeof (IPEndPoint);
            _supportUnmanagement = true;
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
            IPEndPoint value = analyseResult.GetValue<IPEndPoint>(target);
            if (value == null)
            {
                if (!attribute.IsRequire) return;
                throw new PropertyNullValueException(string.Format(ExceptionMessage.EX_PROPERTY_VALUE, attribute.Id, analyseResult.Property.Name, analyseResult.Property.PropertyType));
            }
            proxy.WriteByte((byte)attribute.Id);
            proxy.WriteIPEndPoint(value);
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
            if (target == null) return;
            IPEndPoint value = (IPEndPoint) target;
            proxy.WriteIPEndPoint(value);
        }

        /// <summary>
        /// 从元数据转换为第三方客户数据
        /// </summary>
        /// <param name="attribute">当前字段标注的属性</param><param name="data">元数据</param>
        /// <returns>
        /// 返回转换后的第三方客户数据
        /// </returns>
        /// <exception cref="N:KJFramework.Exception">转换失败</exception>
        public override object Process(IntellectPropertyAttribute attribute, byte[] data)
        {
            unsafe
            {
                fixed (byte* pData = data)
                {
                    IPEndPoint iep = new IPEndPoint(new IPAddress(*(long*)pData), *(int*)(pData + 8));
                    return iep;
                }
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
                    result.SetValue(instance, new IPEndPoint(new IPAddress(*(long*) pData), *(int*) (pData + 8)));
                }
            }
        }

        #endregion
    }
}