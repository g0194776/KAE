using System;
using KJFramework.EventArgs;

namespace KJFramework.Net.Channels
{
    /// <summary>
    ///     Э��ͨ��Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IProtocolChannel : IServiceChannel
    {
        /// <summary>
        ///     ����Э����Ϣ
        /// </summary>
        /// <typeparam name="TMessage">Э����Ϣ����</typeparam>
        /// <returns>����Э����Ϣ</returns>
        TMessage CreateProtocolMessage<TMessage>();
        /// <summary>
        ///     �����¼�
        /// </summary>
        event EventHandler<LightMultiArgEventArgs<Object>> Requested;
        /// <summary>
        ///     �����¼�
        /// </summary>
        event EventHandler<LightMultiArgEventArgs<Object>> Responsed;
    }
}