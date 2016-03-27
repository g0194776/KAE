using System;

namespace KJFramework.Platform.Deploy.CSN.Common.Caches
{
    /// <summary>
    ///     数据缓存元接口，提供了相关的基本操作。
    /// </summary>
    public interface IDataCache<T>
    {
        /// <summary>
        ///     获取或设置缓存的唯一键值
        /// </summary>
        string Key { get; set; }
        /// <summary>
        ///     获取或设置要缓存的项
        /// </summary>
        T Item { get; set; }
        /// <summary>
        ///     获取或设置最后访问时间
        /// </summary>
        DateTime LastVisitTime { get; set; }
        /// <summary>
        ///     获取或设置最后更新时间
        /// </summary>
        DateTime LastUpdateTime { get; set; }
    }
}