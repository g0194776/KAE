using System;
using KJFramework.EventArgs;
using KJFramework.Net.Channels;

namespace KJFramework.Platform.Deploy.CSN.NetworkLayer
{
    /// <summary>
    ///     数据流传输信道元接口，提供了相关的基本操作
    /// </summary>
    public interface ICSNRawTransportChannel : ITransportChannel
    {
        /// <summary>
        ///     获取或设置当前元数据信道是否支持以片段的方式接受网络流数据
        /// </summary>
        bool SupportSegment { get; set; }
        /// <summary>
        ///     接收到数据事件
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<byte[]>> ReceivedData;
        /// <summary>
        ///     接收到数据片段事件
        /// </summary>
        event EventHandler<CSNSegmentReceiveEventArgs> ReceivedDataSegment;
    }
}