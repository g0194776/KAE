using System;
using System.Collections.Generic;
using KJFramework.Enums;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.Enums;
using KJFramework.Statistics;

namespace KJFramework.ServiceModel.Elements
{
    /// <summary>
    ///     绑定元素抽象类，提供了相关的基本操作。
    /// </summary>
    /// <typeparam name="TChannel">通道类型</typeparam>
    public abstract class BindingElement<TChannel> : CommunicationObject, IBindingElement<TChannel>
        where TChannel : IServiceChannel
    {
        #region Implementation of IStatisticable<IStatistic>

        protected Dictionary<StatisticTypes, IStatistic> _statistics = new Dictionary<StatisticTypes, IStatistic>();
        protected bool _enable;
        protected CommunicationStates _communicationState;
        protected Guid _id;
        protected bool _isActive;
        protected double _collectInterval;
        protected Type _collectType;
        protected DateTime _createTime;
        protected object _tag;
        protected InfomationCollectorTypes _infomationCollectorType;
        protected string _name;
        protected bool _canBind;
        protected bool _initialized;

        /// <summary>
        /// 获取或设置统计器
        /// </summary>
        public Dictionary<StatisticTypes, IStatistic> Statistics
        {
            get { return _statistics; }
            set { _statistics = value; }
        }

        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Implementation of ICollector

        /// <summary>
        /// 获取唯一标示
        /// </summary>
        public Guid Id
        {
            get { return _id; }
        }

        /// <summary>
        /// 获取一个值，该值表示了当前收集器是否可用
        /// </summary>
        public bool IsActive
        {
            get { return _isActive; }
        }

        public event EventHandler BeginWork;
        protected void BeginWorkHandler(System.EventArgs e)
        {
            EventHandler work = BeginWork;
            if (work != null) work(this, e);
        }

        public event EventHandler EndWork;
        protected void EndWorkHandler(System.EventArgs e)
        {
            EventHandler work = EndWork;
            if (work != null) work(this, e);
        }

        #endregion

        #region Implementation of IBindingElement<TChannel>

        /// <summary>
        ///     获取绑定元素名称
        /// </summary>
        public string Name
        {
            get { return _name; }
        }
        /// <summary>
        ///     获取一个值，该值标示了当前绑定元素是否可以绑定
        /// </summary>
        public bool CanBind
        {
            get { return _canBind; }
        }
        /// <summary>
        ///     获取一个值，该值表示了当前通道监听器是否已经初始化成功
        /// </summary>
        public bool Initialized
        {
            get { return _initialized; }
        }
        /// <summary>
        ///     初始化
        /// </summary>
        public abstract void Initialize();
        /// <summary>
        ///     创建通道
        /// </summary>
        /// <returns>返回创建后的通道</returns>
        public abstract TChannel CreateChannel();

        #endregion
    }
}