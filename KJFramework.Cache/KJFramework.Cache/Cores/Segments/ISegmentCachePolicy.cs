using System.Collections.Generic;
using KJFramework.Cache.Exception;

namespace KJFramework.Cache.Cores.Segments
{
    /// <summary>
    ///     片段式缓存分布策略元接口，提供了相关的基本操作
    /// </summary>
    public interface ISegmentCachePolicy
    {
        /// <summary>
        ///     获取片段分布等级
        /// </summary>
        int SegmentLevel { get; }
        /// <summary>
        ///     设置一个片段分布策略
        /// </summary>
        /// <param name="size">片段大小</param>
        /// <param name="percent">占用总体内存的百分比</param>
        /// <exception cref="OutOfRangeException">超出预定的范围</exception>
        void Set(int size, decimal percent);
        /// <summary>
        ///     获取所有的片段分布
        /// </summary>
        /// <returns>返回片段分布集合</returns>
        List<SegmentSizePair> Get();
    }
}