using System.Collections.Generic;

namespace KJFramework.Cache.Cores.Segments
{
    /// <summary>
    ///     高速片段缓存元接口，提供了相关的基本操作
    /// </summary>
    internal interface IHightSpeedSegmentCache : ILeasable
    {
        /// <summary>
        ///     获取内部真实缓存数据大小
        /// </summary>
        int RealSize { get; }
        /// <summary>
        ///     获取一个值，该值标示了当前的值是否已经发生了变化
        /// </summary>
        bool Changed { get; }
        /// <summary>
        ///     打入新的片段缓存存根
        /// </summary>
        /// <param name="stub">片段缓存存根</param>
        void Add(ISegmentCacheStub stub);
        /// <summary>
        ///     返回内部数据
        /// </summary>
        /// <returns>内部数据</returns>
        byte[] GetBody();
        /// <summary>
        ///     获取内部所有的缓存存根
        /// </summary>
        /// <returns>返回缓存存根集合</returns>
        IList<ISegmentCacheStub> GetStubs();
    }
}