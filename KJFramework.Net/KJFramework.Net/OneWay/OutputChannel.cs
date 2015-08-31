using System;
using KJFramework.Messages.Contracts;
using KJFramework.Net.ProtocolStacks;

namespace KJFramework.Net.OneWay
{
    /// <summary>
    ///     ���ͨ��Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    /// <typeparam name="T">��Ϣ��������</typeparam>
    class OutputChannel<T> : OnewayChannel<T>, IOutputChannel<T>
        where T : IntellectObject
    {
        #region Constructor

        /// <summary>
        ///     ���ͨ��Ԫ�ӿڣ��ṩ����صĻ���������
        /// </summary>
        /// <param name="protocolStack">Э��ջ</param>
        public OutputChannel(IProtocolStack protocolStack)
            : base(protocolStack)
        {
        }

        /// <summary>
        ///     ���ͨ��Ԫ�ӿڣ��ṩ����صĻ���������
        /// </summary>
        /// <param name="channel">��������ͨѶ�ŵ�</param>
        /// <param name="protocolStack">Э��ջ</param>
        public OutputChannel(IRawTransportChannel channel, IProtocolStack protocolStack)
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
            //use raw transport channel to send current response msg.
            Send(message);
        }

        #endregion

        #region Implementation of IOutputChannel<T>

        /// <summary>
        ///     ����һ����Ϣ��Զ���ս��
        /// </summary>
        /// <param name="message">�������Ϣ</param>
        /// <exception cref="ArgumentNullException">��������Ϊ��</exception>
        /// <exception cref="ArgumentException">��������</exception>
        /// <exception cref="Exception">����ʧ��</exception>
        public int Send(T message)
        {
            if (message == null) throw new ArgumentNullException("message");
            if (!_connected) throw new System.Exception("You cannot call \"Send\" operation on a disconnected channel!");
            message.Bind();
            if (!message.IsBind) throw new ArgumentNullException("Cannot bind a message to binary data, type: " + message);
            return _channel.Send(message.Body);
        }

        /// <summary>
        ///     �첽����һ����Ϣ��Զ���ս��
        /// </summary>
        /// <param name="message">�������Ϣ</param>
        /// <returns>�����첽���</returns>
        /// <exception cref="ArgumentNullException">��������Ϊ��</exception>
        /// <exception cref="ArgumentException">��������</exception>
        /// <exception cref="Exception">����ʧ��</exception>
        public void SendAsync(T message)
        {
            Send(message);
        }

        #endregion
    }
}