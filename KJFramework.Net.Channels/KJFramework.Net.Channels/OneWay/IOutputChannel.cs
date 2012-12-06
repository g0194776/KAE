using KJFramework.Messages.Contracts;

namespace KJFramework.Net.Channels.OneWay
{
    /// <summary>
    ///     ���ͨ��Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    /// <typeparam name="T">��Ϣ��������</typeparam>
    public interface IOutputChannel<T> : IOnewayChannel<T>
        where T : IntellectObject
    {
        /// <summary>
        ///     ����һ����Ϣ��Զ���ս��
        /// </summary>
        /// <param name="message">�������Ϣ</param>
        /// <exception cref="System.ArgumentNullException">��������Ϊ��</exception>
        /// <exception cref="System.ArgumentException">��������</exception>
        /// <exception cref="Exception">����ʧ��</exception>
        int Send(T message);
        /// <summary>
        ///     �첽����һ����Ϣ��Զ���ս��
        /// </summary>
        /// <param name="message">�������Ϣ</param>
        /// <exception cref="System.ArgumentNullException">��������Ϊ��</exception>
        /// <exception cref="System.ArgumentException">��������</exception>
        /// <exception cref="Exception">����ʧ��</exception>
        /// <returns>�����첽���</returns>
        void SendAsync(T message);
    }
}