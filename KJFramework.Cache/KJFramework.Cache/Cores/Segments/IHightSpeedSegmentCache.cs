using System.Collections.Generic;

namespace KJFramework.Cache.Cores.Segments
{
    /// <summary>
    ///     ����Ƭ�λ���Ԫ�ӿڣ��ṩ����صĻ�������
    /// </summary>
    internal interface IHightSpeedSegmentCache : ILeasable
    {
        /// <summary>
        ///     ��ȡ�ڲ���ʵ�������ݴ�С
        /// </summary>
        int RealSize { get; }
        /// <summary>
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰ��ֵ�Ƿ��Ѿ������˱仯
        /// </summary>
        bool Changed { get; }
        /// <summary>
        ///     �����µ�Ƭ�λ�����
        /// </summary>
        /// <param name="stub">Ƭ�λ�����</param>
        void Add(ISegmentCacheStub stub);
        /// <summary>
        ///     �����ڲ�����
        /// </summary>
        /// <returns>�ڲ�����</returns>
        byte[] GetBody();
        /// <summary>
        ///     ��ȡ�ڲ����еĻ�����
        /// </summary>
        /// <returns>���ػ���������</returns>
        IList<ISegmentCacheStub> GetStubs();
    }
}