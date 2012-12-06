namespace KJFramework.Net
{
    /// <summary>
    ///     ӵ���������ӹ��ܵ�Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IReconnectable
    {
        /// <summary>
        ///     ��������
        /// </summary>
        bool Retry();
        /// <summary>
        ///     ��ȡ�������ӵ��ܹ�����
        /// </summary>
        int RetryCount { get; }
        /// <summary>
        ///     ��ȡ�����������Դ���
        /// </summary>
        int RetryIndex { get; set; }
    }
}