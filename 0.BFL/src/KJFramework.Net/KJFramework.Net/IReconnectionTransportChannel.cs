namespace KJFramework.Net
{
    /// <summary>
    ///     ֧�ֶ��������Ĵ���ͨ��Ա�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IReconnectionTransportChannel : ITransportChannel
    {
        /// <summary>
        ///     ���³��Խ�������
        /// </summary>
        /// <returns>���س��Ժ��״̬</returns>
        bool Reconnect();
    }
}