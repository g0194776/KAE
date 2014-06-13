using System;
using System.Collections.Generic;
using KJFramework.Enums;
using KJFramework.Messages.Analysers;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Proxies;
using KJFramework.Statistics;

namespace KJFramework.Messages.TypeProcessors
{
    /// <summary>
    ///     智能的类型处理器抽象父类，提供了相关的基本操作。
    /// </summary>
    public abstract class IntellectTypeProcessor : IIntellectTypeProcessor
    {
        #region 构造函数

        /// <summary>
        ///     智能的类型处理器抽象父类，提供了相关的基本操作。
        /// </summary>
        protected IntellectTypeProcessor()
        {
            _id = Guid.NewGuid();
            //support this act by default.
            _supportUnmanagement = true;
        }

        #endregion

        #region Members

        private Guid _id;
        protected int? _supportedId;
        protected Type _supportedType;
        protected bool _supportUnmanagement;
        protected Dictionary<StatisticTypes,IStatistic> _statistics = new Dictionary<StatisticTypes, IStatistic>();

        #endregion

        #region Implementation of IStatisticable<IStatistic>

        /// <summary>
        /// 获取或设置统计器
        /// </summary>
        public Dictionary<StatisticTypes, IStatistic> Statistics
        {
            get { return _statistics; }
            set { _statistics = value; }
        }

        #endregion

        #region Implementation of IIntellectTypeProcessor

        /// <summary>
        ///     获取唯一编号
        /// </summary>
        public Guid Id
        {
            get { return _id; }
        }

        /// <summary>
        ///     获取一个值，该值标示了当前处理器是否支持以非托管的方式进行执行
        /// </summary>
        public bool SupportUnmanagement
        {
            get { return _supportUnmanagement; }
        }

        /// <summary>
        ///     获取或设置一个值，该值表示了当前需要处理的Id编号。
        ///     <para>* 当一个智能对象的属性集合中存在指定的编号属性，则将会交给此类型处理器处理。</para>
        ///     <para>* 当SupportedId == null时，代表了当前智能类型处理器不关心属性的ID，只关心属性的类型。</para>
        /// </summary>
        public int? SupportedId
        {
            get { return _supportedId; }
            set { _supportedId = value; }
        }

        /// <summary>
        ///     获取支持的类型
        /// </summary>
        public Type SupportedType
        {
            get { return _supportedType; }
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
        public abstract void Process(IMemorySegmentProxy proxy, IntellectPropertyAttribute attribute, ToBytesAnalyseResult analyseResult, object target, bool isArrayElement = false, bool isNullable = false);
        /// <summary>
        ///     从第三方客户数据转换为元数据
        ///     <para>* 此方法将会被轻量级的DataHelper所使用，并且写入的数据将不会拥有编号(Id)</para>
        /// </summary>
        /// <param name="proxy">内存片段代理器</param>
        /// <param name="target">目标对象实例</param>
        /// <param name="isArrayElement">当前写入的值是否为数组元素标示</param>
        /// <param name="isNullable">是否为可空字段标示</param>
        public abstract void Process(IMemorySegmentProxy proxy, object target, bool isArrayElement = false, bool isNullable = false);
        /// <summary>
        ///     从元数据转换为第三方客户数据
        /// </summary>
        /// <param name="attribute">当前字段标注的属性</param>
        /// <param name="data">元数据</param>
        /// <returns>返回转换后的第三方客户数据</returns>
        /// <exception cref="Exception">转换失败</exception>
        public abstract object Process(IntellectPropertyAttribute attribute, byte[] data);
        /// <summary>
        ///     从元数据转换为第三方客户数据
        /// </summary>
        /// <param name="instance">目标对象</param>
        /// <param name="result">分析结果</param>
        /// <param name="data">元数据</param>
        /// <param name="offset">元数据所在的偏移量</param>
        /// <param name="length">元数据长度</param>
        public abstract void Process(object instance, GetObjectAnalyseResult result, byte[] data, int offset, int length = 0);

        #endregion
    }
}