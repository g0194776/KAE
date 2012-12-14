using System;
using System.Collections.Generic;
using KJFramework.Basic.Enum;
using KJFramework.Net.Channel;
using KJFramework.Net.Channels.Enums;
using KJFramework.Statistics;
using KJFramework.Tracing;

namespace KJFramework.Net.Channels
{
    /// <summary>
    ///     服务通道抽象基类，提供了相关的基本操作。
    /// </summary>
    public abstract class ServiceChannel : IServiceChannel
    {
        #region 构造函数

        /// <summary>
        ///     服务通道抽象基类，提供了相关的基本操作。
        /// </summary>
        protected ServiceChannel()
        {
            _createTime = DateTime.Now;
            _key = Guid.NewGuid();
        }

        #endregion

        #region 方法

        /// <summary>
        ///     停止
        /// </summary>
        protected abstract void InnerAbort();
        /// <summary>
        ///     打开
        /// </summary>
        protected abstract void InnerOpen();
        /// <summary>
        ///     关闭
        /// </summary>
        protected abstract void InnerClose();

        #endregion

        #region Members

        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof (ServiceChannel));

        #endregion

        #region Implementation of IChannel<BasicChannelInfomation>

        protected BasicChannelInfomation _channelInfo;
        protected Dictionary<StatisticTypes, IStatistic> _statistics;
        protected bool _enable;
        protected CommunicationStates _communicationState;
        protected DateTime _createTime;
        protected Guid _key;
        protected int _id;

        /// <summary>
        /// 获取或设置当前通道信息
        /// </summary>
        public BasicChannelInfomation ChannelInfo
        {
            get { return _channelInfo; }
            set { _channelInfo = value; }
        }

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

        #region Implementation of ICommunicationObject

        /// <summary>
        ///     停止
        /// </summary>
        public void Abort()
        {
            try
            {
                _communicationState = CommunicationStates.Closing;
                InnerAbort();
                _communicationState = CommunicationStates.Closed;
            }
            catch (System.Exception ex)
            {
                _communicationState = CommunicationStates.Faulte;
                _tracing.Error(ex, null);
                FaultedHandler(null);
                throw;
            }
        }

        /// <summary>
        ///     打开
        /// </summary>
        public void Open()
        {
            try
            {
                _communicationState = CommunicationStates.Opening;
                OpeningHandler(null);
                InnerOpen();
                _communicationState = CommunicationStates.Opened;
                OpenedHandler(null);
            }
            catch (System.Exception ex)
            {
                _communicationState = CommunicationStates.Faulte;
                _tracing.Error(ex, null);
                FaultedHandler(null);
                throw;
            }
        }
        /// <summary>
        ///     关闭
        /// </summary>
        public void Close()
        {
            try
            {
                _communicationState = CommunicationStates.Closing;
                ClosingHandler(null);
                InnerClose();
                _communicationState = CommunicationStates.Closed;
                ClosedHandler(null);
            }
            catch (System.Exception ex)
            {
                _communicationState = CommunicationStates.Faulte;
                _tracing.Error(ex, null);
                FaultedHandler(null);
                throw;
            }
        }

        /// <summary>
        ///     异步打开
        /// </summary>
        /// <param name="callback">回调函数</param>
        /// <param name="state">状态</param>
        /// <returns>返回异步结果</returns>
        public virtual IAsyncResult BeginOpen(AsyncCallback callback, object state)
        {
            Action action = Open;
            return action.BeginInvoke(callback, state);
        }
        /// <summary>
        ///     异步关闭
        /// </summary>
        /// <param name="callback">回调函数</param>
        /// <param name="state">状态</param>
        /// <returns>返回异步结果</returns>
        public virtual IAsyncResult BeginClose(AsyncCallback callback, object state)
        {
            Action action = Close;
            return action.BeginInvoke(callback, state);
        }
        /// <summary>
        ///     异步打开
        /// </summary>
        public virtual void EndOpen(IAsyncResult result)
        {
            if (result != null)
            {
                Action action = (Action) result.AsyncState;
                action.EndInvoke(result);
            }
        }
        /// <summary>
        ///     异步关闭
        /// </summary>
        public virtual void EndClose(IAsyncResult result)
        {
            if (result != null)
            {
                Action action = (Action)result.AsyncState;
                action.EndInvoke(result);
            }
        }

        /// <summary>
        ///     获取或设置当前可用状态
        /// </summary>
        public bool Enable
        {
            get { return _enable; }
            set { _enable = value; }
        }

        /// <summary>
        ///     获取当前通讯状态
        /// </summary>
        public CommunicationStates CommunicationState
        {
            get { return _communicationState; }
        }

        /// <summary>
        ///     已关闭事件
        /// </summary>
        public event EventHandler Closed;
        protected void ClosedHandler(System.EventArgs e)
        {
            EventHandler closed = Closed;
            if (closed != null) closed(this, e);
        }

        /// <summary>
        ///     正在关闭事件
        /// </summary>
        public event EventHandler Closing;
        protected void ClosingHandler(System.EventArgs e)
        {
            EventHandler closing = Closing;
            if (closing != null) closing(this, e);
        }

        /// <summary>
        ///     已错误事件
        /// </summary>
        public event EventHandler Faulted;
        protected void FaultedHandler(System.EventArgs e)
        {
            EventHandler faulted = Faulted;
            if (faulted != null) faulted(this, e);
        }

        /// <summary>
        ///     已开启事件
        /// </summary>
        public event EventHandler Opened;
        protected void OpenedHandler(System.EventArgs e)
        {
            EventHandler opened = Opened;
            if (opened != null) opened(this, e);
        }

        /// <summary>
        ///     正在开启事件
        /// </summary>
        public event EventHandler Opening;
        protected void OpeningHandler(System.EventArgs e)
        {
            EventHandler opening = Opening;
            if (opening != null) opening(this, e);
        }

        #endregion

        #region Implementation of IServiceChannel

        /// <summary>
        ///     获取创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get { return _createTime; }
        }

        /// <summary>
        ///     获取通道唯一标示
        /// </summary>
        public Guid Key
        {
            get { return _key; }
        }

        /// <summary>
        ///     获取或设置通道编号
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        #endregion
    }
}