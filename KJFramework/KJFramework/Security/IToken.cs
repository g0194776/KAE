namespace KJFramework.Security
{
    /// <summary>
    ///     ����Ԫ�ӿڣ��ṩ����ص����Խṹ��
    /// </summary>
    public interface IToken
    {
        /// <summary>
        ///     ��ȡ��������
        /// </summary>
        byte[] Content { get; }
    }
}