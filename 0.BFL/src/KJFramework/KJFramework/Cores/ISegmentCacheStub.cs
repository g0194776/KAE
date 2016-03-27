using KJFramework.Indexers;

namespace KJFramework.Cores
{
    /// <summary>
    ///     �ڴ�λ�����Ԫ�ӿڣ��ṩ����صĻ�������
    /// </summary>
    internal interface ISegmentCacheStub : ICacheStub<byte[]>
    {
        /// <summary>
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰ�Ļ����Ƿ��Ѿ���ʹ��
        /// </summary>
        bool IsUsed { get; }
        /// <summary>
        ///     ��ȡ����������
        /// </summary>
        ICacheIndexer Indexer { get; }
        /// <summary>
        ///     ��ʼ��
        /// </summary>
        void Initialize();
    }
}