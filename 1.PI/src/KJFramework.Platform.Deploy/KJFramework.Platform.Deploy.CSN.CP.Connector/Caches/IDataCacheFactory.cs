namespace KJFramework.Platform.Deploy.CSN.CP.Connector.Caches
{
    /// <summary>
    ///     ���ݻ��湤��Ԫ�ӿڣ��ṩ����صĻ�������
    /// </summary>
    public interface IDataCacheFactory<T>
    {
        /// <summary>
        ///     ����һ���������
        /// </summary>
        /// <param name="args">����������������</param>
        /// <returns>���ش����Ļ������</returns>
        IDataCache<T> Create(params object[] args);
    }
}