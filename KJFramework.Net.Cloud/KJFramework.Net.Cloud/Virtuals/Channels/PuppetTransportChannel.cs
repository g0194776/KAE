using System.Net;
using KJFramework.Net.Channel;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.Enums;

namespace KJFramework.Net.Cloud.Virtuals.Channels
{
    /// <summary>
    ///     ���ܴ���ͨ�����ṩ����صĻ�������
    /// </summary>
    public class PuppetTransportChannel : TransportChannel
    {
        #region Constructor

        private IPEndPoint _localEndPoint;

        private IPEndPoint _remoteEndPoint;

        /// <summary>
        ///     ���ܴ���ͨ�����ṩ����صĻ�������
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
        /// ��ȡ�����ս���ַ
        /// </summary>
        public override IPEndPoint LocalEndPoint
        {
            get { return _localEndPoint; }
        }

        /// <summary>
        /// ��ȡԶ���ս���ַ
        /// </summary>
        public override IPEndPoint RemoteEndPoint
        {
            get { return _remoteEndPoint; }
        }

        /// <summary>
        /// ��������
        /// <para>
        /// * ����˷������з��͵�Ԫ���ݣ��������Զ��ְ�������ݡ�
        /// </para>
        /// </summary>
        /// <param name="data">Ҫ���͵�����</param>
        /// <returns>
        /// ���ط��͵��ֽ���
        /// </returns>
        protected override int InnerSend(byte[] data)
        {
            return 0;
        }

        #endregion
    }
}