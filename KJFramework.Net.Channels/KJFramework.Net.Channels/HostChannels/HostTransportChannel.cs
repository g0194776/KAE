using System;
using System.Collections.Generic;
using KJFramework.Enums;
using KJFramework.EventArgs;
using KJFramework.Statistics;

namespace KJFramework.Net.Channels.HostChannels
{
    /// <summary>
    ///     ËÞÖ÷´«ÊäÍ¨µÀ³éÏó¸¸Àà£¬Ìá¹©ÁËÏà¹ØµÄ»ù±¾²Ù×÷¡£
    /// </summary>
    public abstract class HostTransportChannel : IHostTransportChannel
    {
        #region Constructor

        /// <summary>
        ///     ËÞÖ÷´«ÊäÍ¨µÀ³éÏó¸¸Àà£¬Ìá¹©ÁËÏà¹ØµÄ»ù±¾²Ù×÷¡£
        /// </summary>
        protected HostTransportChannel()
        {
            _id = Guid.NewGuid();
        }

        #endregion

        #region Members

        protected readonly Guid _id;
        protected Dictionary<StatisticTypes, IStatistic> _statistics;
        /// <summary>
        ///     获取或设置附属标记
        /// </summary>
        public object Tag { get; set; }

        #endregion

        #region Implementation of IHostTransportChannel

        /// <summary>
        ///     »ñÈ¡Î¨Ò»±êÊ¶
        /// </summary>
        public Guid Id
        {
            get { return _id; }
        }
        /// <summary>
        ///     ×¢²áÍøÂç
        /// </summary>
        /// <returns>·µ»Ø×¢²áµÄ×´Ì¬</returns>
        public abstract bool Regist();
        /// <summary>
        ///     ×¢ÏúÍøÂç
        /// </summary>
        /// <returns>·µ»Ø×¢²áµÄ×´Ì¬</returns>
        public abstract bool UnRegist();

        #endregion

        #region ÊÂ¼þ

        /// <summary>
        ///     ´´½¨Í¨µÀÊÂ¼þ
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<ITransportChannel>> ChannelCreated;
        protected void ChannelCreatedHandler(LightSingleArgEventArgs<ITransportChannel> e)
        {
            EventHandler<LightSingleArgEventArgs<ITransportChannel>> created = ChannelCreated;
            if (created != null) created(this, e);
        }
        /// <summary>
        ///     ¶Ï¿ªÍ¨µÀÊÂ¼þ
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