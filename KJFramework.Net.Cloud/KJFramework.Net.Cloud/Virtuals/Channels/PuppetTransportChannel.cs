using System.Net;
using KJFramework.Net.Channel;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.Enums;

namespace KJFramework.Net.Cloud.Virtuals.Channels
{
    /// <summary>
    ///     傀儡传输通道，提供了相关的基本操作
    /// </summary>
    public class PuppetTransportChannel : TransportChannel
    {
        #region Constructor

        private IPEndPoint _localEndPoint;

        private IPEndPoint _remoteEndPoint;

        /// <summary>
        ///     傀儡传输通道，提供了相关的基本操作
        /// </summary>
        public PuppetTransportChannel()
        {
            _channelInfo = new BasicChannelInfomation();
        }

        #endregion

        #region Overrides of ServiceChannel

        protected override void InnerAbort()
        {
            _communicationState = CommunicationStates.Closed;
        }

        protected override void InnerOpen()
        {
            _communicationState = CommunicationStates.Opened;
        }

        protected override void InnerClose()
        {
            _communicationState = CommunicationStates.Closed;
        }

        #endregion

        #region Overrides of TransportChannel

        public override void Connect()
        {
            _connected = true;
            ConnectedHandler(null);
        }

        public override void Disconnect()
        {
            _connected = false;
            DisconnectedHandler(null);
        }

        /// <summary>
        /// 获取本地终结点地址
        /// </summary>
        public override IPEndPoint LocalEndPoint
        {
            get { return _localEndPoint; }
        }

        /// <summary>
        /// 获取远程终结点地址
        /// </summary>
        public override IPEndPoint RemoteEndPoint
        {
            get { return _remoteEndPoint; }
        }

        /// <summary>
        /// 发送数据
        /// <para>
        /// * 如果此方法进行发送的元数据，可能是自动分包后的数据。
        /// </para>
        /// </summary>
        /// <param name="data">要发送的数据</param>
        /// <returns>
        /// 返回发送的字节数
        /// </returns>
        protected override int InnerSend(byte[] data)
        {
            return 0;
        }

        #endregion
    }
}