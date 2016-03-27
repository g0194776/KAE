namespace KJFramework.Cores
{
    /// <summary>
    ///     ������Ԫ�ӿڣ��ṩ����صĻ�������
    /// </summary>
    /// <typeparam name="T">��������</typeparam>
    public interface ICacheStub<T>
    {
        /// <summary>
        ///     ��ȡ��ǰ���������ڲ����
        /// </summary>
        int Id { get; }
        /// <summary>
        ///     ��ȡ������һ��ֵ����ֵ��ʾ�˵�ǰ�������Ƿ��ʾΪһ�ֹ�̬�Ļ���״̬
        /// </summary>
        bool Fixed { get; set; }
        /// <summary>
        ///     ��ȡ�����û�����
        /// </summary>
        ICacheItem<T> Cache { get; set; }
        /// <summary>
        ///     ��ȡ������������
        /// </summary>
        /// <returns></returns>
        ICacheLease GetLease();
    }
}