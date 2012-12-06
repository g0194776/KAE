using System;
using System.Collections.Generic;
using KJFramework.Basic.Enum;
using KJFramework.EventArgs;
using KJFramework.Statistics;

namespace KJFramework.Net.Channels.HostChannels
{
    /// <summary>
    ///     宿主传输通道抽象父类，提供了相关的基本操作。
    /// </summary>
    public abstract class HostTransportChannel : IHostTransportChannel
    {
        #region Constructor

        /// <summary>
        ///     宿主传输通道抽象父类，提供了相关的基本操作。
        /// </summary>
        protected HostTransportChannel()
        {
            _id = Guid.NewGuid();
        }

        #endregion

        #region Members

        protected readonly Guid _id;
        protected Dictionary<StatisticTypes, IStatistic> _statistics;

        #endregion

        #region Implementation of IHostTransportChannel

        /// <summary>
        ///     获取唯一标识
        /// </summary>
        public Guid Id
        {
            get { return _id; }
        }
        /// <summary>
        ///     注册网络
        /// </summary>
        /// <returns>返回注册的状态</returns>
        public abstract bool Regist();
        /// <summary>
        ///     注销网络
        /// </summary>
        /// <returns>返回注册的状态</returns>
        public abstract bool UnRegist();

        #endregion

        #region 事件

        /// <summary>
        ///     创建通道事件
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<ITransportChannel>> ChannelCreated;
        protected void ChannelCreatedHandler(LightSingleArgEventArgs<ITransportChannel> e)
        {
            EventHandler<LightSingleArgEventArgs<ITransportChannel>> created = ChannelCreated;
            if (created != null) created(this, e);
        }
        /// <summary>
        ///     断开通道事件
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<ITransportChannel>> ChannelDisconnected;
        protected void ChannelDisconnectedHandler(LightSingleArgEventArgs<ITransportChannel> e)
        {
            EventHandler<LightSingleArgEventArgs<ITransportChannel>> handler = ChannelDisconnected;
            if (handler != null) handler(this, e);
        }

        #endregion
    }
}