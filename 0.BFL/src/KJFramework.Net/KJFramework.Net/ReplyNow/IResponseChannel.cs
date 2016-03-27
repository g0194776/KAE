using System;

namespace KJFramework.Net.ReplyNow
{
    /// <summary>
    ///     Ӧ��ͨ�����ṩ����صĻ�������
    /// </summary>
    public interface IResponseChannel : ICommunicationChannelAddress, IProtocolChannel
    {
        /// <summary>
        ///     Ӧ��һ����Ϣ��Զ���ս��
        /// </summary>
        /// <typeparam name="TMessage">��Ϣ����</typeparam>
        /// <param name="message">�������Ϣ</param>
        /// <returns>����Ӧ����Ϣ</returns>
        TMessage Response<TMessage>(TMessage message);
        /// <summary>
        ///     �첽Ӧ��һ����Ϣ��Զ���ս��
        /// </summary>
        /// <typeparam name="TMessage">��Ϣ����</typeparam>
        /// <param name="message">�������Ϣ</param>
        /// <param name="callback">�ص�����</param>
        /// <param name="state">״̬</param>
        /// <returns>�����첽���</returns>
        IAsyncResult BeginResponse<TMessage>(TMessage message, AsyncCallback callback, Object state);
        /// <summary>
        ///     �첽Ӧ��һ����Ϣ��Զ���ս��
        /// </summary>
        /// <typeparam name="TMessage">��Ϣ����</typeparam>
        /// <param name="result">�첽���</param>
        /// <returns>����Ӧ����Ϣ</returns>
        TMessage EndResponse<TMessage>(IAsyncResult result);
    }
}