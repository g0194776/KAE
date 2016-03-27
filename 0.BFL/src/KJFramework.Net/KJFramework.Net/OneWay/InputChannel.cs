using System;
using KJFramework.Messages.Contracts;
using KJFramework.Net.ProtocolStacks;

namespace KJFramework.Net.OneWay
{
    /// <summary>
    ///     ����ͨ��Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    /// <typeparam name="T">��Ϣ��������</typeparam>
    class InputChannel<T> : OnewayChannel<T> 
        where T : IntellectObject
    {
        #region Constructor

        /// <summary>
        ///     ����ͨ��Ԫ�ӿڣ��ṩ����صĻ���������
        /// </summary>
        /// <param name="protocolStack">Э��ջ</param>
        public InputChannel(IProtocolStack protocolStack)
            : base(protocolStack)
        {
        }

        /// <summary>
        ///     ����ͨ��Ԫ�ӿڣ��ṩ����صĻ���������
        /// </summary>
        /// <param name="channel">��������ͨѶ�ŵ�</param>
        /// <param name="protocolStack">Э��ջ</param>
        public InputChannel(IRawTransportChannel channel, IProtocolStack protocolStack)
            : base(channel, protocolStack)
        {
        }

        #endregion

        #region Overrides of OnewayChannel<T>

        /// <summary>
        ///     ʹ�ô˷�����������Ӧ��Ϣ
        /// </summary>
        /// <param name="message">��Ӧ��Ϣ</param>
        protected override void SendCallbackAsync(T message)
        {
            if (!_connected || message == null) return;
            message.Bind();
            if (!message.IsBind) throw new ArgumentNullException("Cannot bind a message to binary data, type: " + message);
            _channel.Send(message.Body);
        }

        #endregion
    }
}