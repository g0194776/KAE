namespace KJFramework.Cores
{
    /// <summary>
    ///     ֻ��������Ԫ�ӿڣ��ṩ����صĻ�������
    /// </summary>
    /// <typeparam name="T">��������</typeparam>
    public interface IReadonlyCacheStub<T>
    {
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