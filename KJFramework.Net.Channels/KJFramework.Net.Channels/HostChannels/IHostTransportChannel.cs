using System;
using KJFramework.EventArgs;

namespace KJFramework.Net.Channels.HostChannels
{
    /// <summary>
    ///     ËÞÖ÷´«ÊäÍ¨µÀÔª½Ó¿Ú£¬Ìá¹©ÁËÏà¹ØµÄ»ù±¾²Ù×÷¡£
    /// </summary>
    public interface IHostTransportChannel
    {
        /// <summary>
        ///     获取或设置附属标记
        /// </summary>
        object Tag { get; set; }
        /// <summary>
        ///     »ñÈ¡Î¨Ò»±êÊ¶
        /// </summary>
        Guid Id { get; }
        /// <summary>
        ///     ×¢²áÍøÂç
        /// </summary>
        /// <returns>·µ»Ø×¢²áµÄ×´Ì¬</returns>
        bool Regist();
        /// <summary>
        ///     ×¢ÏúÍøÂç
        /// </summary>
        /// <returns>·µ»Ø×¢²áµÄ×´Ì¬</returns>
        bool UnRegist();
        /// <summary>
        ///     ´´½¨Í¨µÀÊÂ¼þ
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<ITransportChannel>> ChannelCreated;
        /// <summary>
        ///     Í¨µÀ¶Ï¿ªÊÂ¼þ
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<ITransportChannel>> ChannelDisconnected;
    }
}