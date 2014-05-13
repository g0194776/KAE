using System;
using KJFramework.Enums;
using KJFramework.EventArgs;

namespace KJFramework.Diagnostics
{
    /// <summary>
    ///     信息收集器基类，提供了相关的基本操作。
    /// </summary>
    public class InfomationCollector : IInfomationCollector
    {
        #region 构造函数

        /// <summary>
        ///     信息收集器抽象类，提供了相关的基本操作。
        /// </summary>
        /// <param name="collectType">收集对象</param>
        /// <param name="reviewere">审查器</param>
        protected InfomationCollector(Type collectType, IInfomationReviewer reviewere)
        {
            _collectType = collectType;
            _reviewer = reviewere;
            _createTime = DateTime.Now;
            _id = Guid.NewGuid();
        }

        #endregion

        #region 析构函数

        ~InfomationCollector()
        {
            Dispose();
        }

        #endregion

        #region 成员

        protected System.Timers.Timer _timer;

        #endregion

        #region Implementation of IControlable

        protected IInfomationReviewer _reviewer;
        protected Guid _id;
        protected bool _isActive;
        protected Type _collectType;
        protected DateTime _createTime;
        protected object _tag;
        protected InfomationCollectorTypes _infomationCollectorType;

        /// <summary>
        ///     开始执行
        /// </summary>
        public virtual void Start()
        {
            BeginWorkHandler(null);
        }
        /// <summary>
        ///     停止执行
        /// </summary>
        public virtual void Stop()
        {
            EndWorkHandler(null);
        }

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Implementation of ICollector

        /// <summary>
        ///     获取唯一标示
        /// </summary>
        public Guid Id
        {
            get { return _id; }
        }

        /// <summary>
        ///     获取一个值，该值表示了当前收集器是否可用
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

        #region Implementation of IInfomationCollector

        protected double _collectInterval = 5000;

        /// <summary>
        ///     收集时间间隔
        ///            * 时间单位： 毫秒。
        /// </summary>
        public double CollectInterval
        {
            get { return _collectInterval; }
            set { _collectInterval = value; }
        }

        /// <summary>
        ///     获取或设置被收集信息的对象类型
        /// </summary>
        public Type CollectType
        {
            get { return _collectType; }
        }

        /// <summary>
        ///     获取创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get { return _createTime; }
        }

        /// <summary>
        ///     获取或设置附属属性
        /// </summary>
        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        /// <summary>
        ///     获取信息收集器类型枚举
        /// </summary>
        public InfomationCollectorTypes InfomationCollectorType
        {
            get { return _infomationCollectorType; }
        }

        /// <summary>
        ///     获取评审器
        /// </summary>
        /// <returns>返回评审器</returns>
        public IInfomationReviewer GetReviewer()
        {
            return _reviewer;
        }

        /// <summary>
        ///     重置收集时间
        /// </summary>
        /// <param name="interval">收集时间</param>
        public void ResetCollectInterval(double interval)
        {
            if (interval > 0)
            {
                _collectInterval = interval;
                IntervalTimeChangedHandler(null);
            }
        }

        /// <summary>
        ///     新信息事件
        /// </summary>
        public event EventHandler<NewInfomationEventArgs> NewInfomation;
        /// <summary>
        ///     收集时间被重置事件
        /// </summary>
        public event EventHandler IntervalTimeChanged;
        protected void IntervalTimeChangedHandler(System.EventArgs e)
        {
            EventHandler changed = IntervalTimeChanged;
            if (changed != null) changed(this, e);
        }
        protected void NewInfomationHandler(NewInfomationEventArgs e)
        {
            EventHandler<NewInfomationEventArgs> infomation = NewInfomation;
            if (infomation != null) infomation(this, e);
        }

        #endregion

        #region 父类方法

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return String.Format("Infomation Collector - {0}.", _collectType.FullName);
        }

        #endregion
    }
}