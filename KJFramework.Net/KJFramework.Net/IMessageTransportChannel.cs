using System;
using System.Collections.Generic;
using KJFramework.EventArgs;
using KJFramework.Net.Identities;
using KJFramework.Net.Managers;
using KJFramework.Net.ProtocolStacks;

namespace KJFramework.Net
{
    /// <summary>
    ///     ��Ϣ�����ŵ�Ԫ�ӿڣ��ṩ����صĻ�������
    /// </summary>
    public interface IMessageTransportChannel<T> : ITransportChannel
    {
        /// <summary>
        ///     ��ȡЭ��ջ
        /// </summary>
        IProtocolStack ProtocolStack { get; }
        /// <summary>
        ///     ��ȡ�����÷��Ƭ��Ϣ������
        /// </summary>
        IMultiPacketManager<T> MultiPacketManager { get; set; }
        /// <summary>
        ///     ����һ����Ϣ
        /// </summary>
        /// <param name="obj">Ҫ���͵���Ϣ</param>
        /// <returns>���ط��͵��ֽ���</returns>
        int Send(T obj);
        /// <summary>
        ///     ���յ���Ϣ�¼�
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<List<T>>> ReceivedMessage;
        /// <summary>
        ///   ����һ�����������Ψһ��ʾ
        /// </summary>
        /// <param name="messageId">��Ϣ���</param>
        /// <returns>���ش����������Ψһ��ʾ</returns>
        TransactionIdentity GenerateRequestIdentity(uint messageId);
    }
}