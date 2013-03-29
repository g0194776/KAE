using System;
using KJFramework.Messages.Analysers;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Proxies;
using KJFramework.Statistics;

namespace KJFramework.Messages.TypeProcessors
{
    /// <summary>
    ///     智能的类型处理器元接口，提供了对于特定类型特定处理能力的基础支持。
    /// </summary>
    public interface IIntellectTypeProcessor : IStatisticable<IStatistic>
    {
        /// <summary>
        ///     获取唯一编号
        /// </summary>
        Guid Id { get; }
        /// <summary>
        ///     获取一个值，该值标示了当前处理器是否支持以非托管的方式进行执行
        /// </summary>
        bool SupportUnmanagement { get; }
        /// <summary>
        ///     获取或设置一个值，该值表示了当前需要处理的Id编号。
        ///     <para>* 当一个智能对象的属性集合中存在指定的编号属性，则将会交给此类型处理器处理。</para>
        ///     <para>* 当SupportedId == null时，代表了当前智能类型处理器不关心属性的ID，只关心属性的类型。</para>
        /// </summary>
        int? SupportedId { get; set; }
        /// <summary>
        ///     获取支持的类型
        /// </summary>
        Type SupportedType { get; }
        /// <summary>
        ///     从第三方客户数据转换为元数据
        /// </summary>
        /// <param name="proxy">内存片段代理器</param>
        /// <param name="attribute">字段属性</param>
        /// <param name="analyseResult">分析结果</param>
        /// <param name="target">目标对象实例</param>
        /// <param name="isArrayElement">当前写入的值是否为数组元素标示</param>
        /// <param name="isNullable">是否为可空字段标示</param>
        void Process(IMemorySegmentProxy proxy, IntellectPropertyAttribute attribute, ToBytesAnalyseResult analyseResult, object target, bool isArrayElement = false, bool isNullable = false);
        /// <summary>
        ///     从第三方客户数据转换为元数据
        ///     <para>* 此方法将会被轻量级的DataHelper所使用，并且写入的数据将不会拥有编号(Id)</para>
        /// </summary>
        /// <param name="proxy">内存片段代理器</param>
        /// <param name="target">目标对象实例</param>
        /// <param name="isArrayElement">当前写入的值是否为数组元素标示</param>
        /// <param name="isNullable">是否为可空字段标示</param>
        void Process(IMemorySegmentProxy proxy, object target, bool isArrayElement = false, bool isNullable = false);
        /// <summary>
        ///     从元数据转换为第三方客户数据
        /// </summary>
        /// <param name="attribute">当前字段标注的属性</param>
        /// <param name="data">元数据</param>
        /// <returns>返回转换后的第三方客户数据</returns>
        /// <exception cref="Exception">转换失败</exception>
        Object Process(IntellectPropertyAttribute attribute, byte[] data);
        /// <summary>
        ///     从元数据转换为第三方客户数据
        /// </summary>
        /// <param name="instance">目标对象</param>
        /// <param name="result">分析结果</param>
        /// <param name="data">元数据</param>
        /// <param name="offset">元数据所在的便宜量</param>
        /// <param name="length">元数据长度</param>
        void Process(Object instance, GetObjectAnalyseResult result, byte[] data, int offset, int length = 0);
    }
}