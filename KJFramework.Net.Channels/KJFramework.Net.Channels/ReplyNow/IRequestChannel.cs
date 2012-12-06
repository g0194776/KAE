using System;

namespace KJFramework.Net.Channels.ReplyNow
{
    /// <summary>
    ///     ����ͨ�����ṩ����صĻ�������
    /// </summary>
    public interface IRequestChannel : ICommunicationChannelAddress, IProtocolChannel
    {
        /// <summary>
        ///     ����һ����Ϣ��Զ���ս��
        /// </summary>
        /// <typeparam name="T">��Ϣ����</typeparam>
        /// <param name="message">�������Ϣ</param>
        /// <returns>����Ӧ����Ϣ</returns>
        T Request<T>(T message);
        /// <summary>
        ///     �첽����һ����Ϣ��Զ���ս��
        /// </summary>
        /// <typeparam name="T">��Ϣ����</typeparam>
        /// <param name="message">�������Ϣ</param>
        /// <param name="callback">�ص�����</param>
        /// <param name="state">״̬</param>
        /// <returns>�����첽���</returns>
        IAsyncResult BeginRequest<T>(T message, AsyncCallback callback, Object state);
        /// <summary>
        ///     �첽����һ����Ϣ��Զ���ս��
        /// </summary>
        /// <typeparam name="T">��Ϣ����</typeparam>
        /// <param name="result">�첽���</param>
        /// <returns>����Ӧ����Ϣ</returns>
        T EndRequest<T>(IAsyncResult result);
    }
}