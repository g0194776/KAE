using KJFramework.Cache.Cores;

namespace KJFramework.Cache.Containers
{
    /// <summary>
    ///     �̶������Ļ�������Ԫ�ӿڣ��ṩ����صĻ�������
    /// </summary>
    /// <typeparam name="T">��������</typeparam>
    public interface IFixedCacheContainer<T>
        where T : IClearable, new()
    {
        /// <summary>
        ///     ��ȡ��ǰ�������������
        /// </summary>
        int Capacity { get; }
        /// <summary>
        ///     ���һ������
        /// </summary>
        /// <returns>����һ���µĻ���</returns>
        IFixedCacheStub<T> Rent();
        /// <summary>
        ///     �黹һ������
        /// </summary>
        /// <param name="cache">����</param>
        void Giveback(IFixedCacheStub<T> cache);
        /// <summary>
        ///    �����ڲ����ܼ�����
        /// </summary>
        /// <param name="name">���ܼ���������</param>
        void BuildPerformanceCounter(string name);
    }
}