namespace KJFramework.Cores
{
    /// <summary>
    ///     ��̬������Ԫ�ӿڣ��ṩ����صĻ�������
    /// </summary>
    /// <typeparam name="T">��������</typeparam>
    public interface IFixedCacheStub<T>
    {
        /// <summary>
        ///     ��ȡ�����ø�������
        /// </summary>
        object Tag { get; set; }
        /// <summary>
        ///     ��ȡ����
        /// </summary>
        T Cache { get; }
        /// <summary>
        ///     ��ȡ�������������
        /// </summary>
        ICacheLease Lease { get; }
    }
}