namespace KJFramework.Messages.Contracts
{
    /// <summary>
    ///     δ֪����Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IUnknownParameter
    {
        /// <summary>
        ///     ��ȡ�������
        /// </summary>
        int Id { get; }
        /// <summary>
        ///     ��ȡ����Ԫ����
        /// </summary>
        byte[] Content { get; }
    }
}