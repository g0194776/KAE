namespace KJFramework.Indexers
{
    /// <summary>
    ///     ����������Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    internal interface ICacheIndexer
    {
        /// <summary>
        ///     ��ȡ��������ʼ��ƫ����
        /// </summary>
        int BeginOffset { get; }
        /// <summary>
        ///     ��ȡ��ǰ�������ݶδ�С
        /// </summary>
        int SegmentSize { get; }
        /// <summary>
        ///     ��ȡ���滺����
        /// </summary>
        byte[] CacheBuffer { get; }
    }
}