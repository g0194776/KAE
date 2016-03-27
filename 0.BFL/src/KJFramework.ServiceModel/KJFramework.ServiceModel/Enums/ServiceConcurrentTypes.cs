namespace KJFramework.ServiceModel.Enums
{
    /// <summary>
    ///     ����ʵ������ö��
    ///     <para>* ��ö�����;����ŷ�������ʱ�Ĳ���Ч����</para>
    /// </summary>
    public enum ServiceConcurrentTypes
    {
        /// <summary>
        ///     ��һʵ��
        /// </summary>
        Singleton,
        /// <summary>
        ///     ���̲߳���
        /// </summary>
        Multi,
        /// <summary>
        ///     ��˲���
        /// </summary>
        Concurrent,
    }
}