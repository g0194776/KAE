using System;
using KJFramework.EventArgs;

namespace KJFramework.Cache.Containers
{
    /// <summary>
    ///     片段式缓存容器元接口，提供了相关的基本操作
    /// </summary>
    /// <typeparam name="T">唯一标识类型</typeparam>
    public interface ISegmentCacheContainer<T>
    {
        /// <summary>
        ///     获取最大容量
        /// </summary>
        int Capacity { get; }
        /// <summary>
        ///     添加一个缓存
        /// </summary>
        /// <param name="key">缓存唯一标识</param>
        /// <param name="obj">缓存数据</param>
        /// <returns>返回添加后的标示</returns>
        bool Add(T key, byte[] obj);
        /// <summary>
        ///     添加一个缓存
        /// </summary>
        /// <param name="key">缓存唯一标识</param>
        /// <param name="obj">缓存数据</param>
        /// <param name="timeSpan">续租时间</param>
        /// <returns>返回添加后的标示</returns>
        bool Add(T key, byte[] obj, TimeSpan timeSpan);
        /// <summary>
        ///     添加一个缓存
        /// </summary>
        /// <param name="key">缓存唯一标识</param>
        /// <param name="obj">缓存数据</param>
        /// <param name="expireTime">过期时间</param>
        /// <returns>返回添加后的标示</returns>
        bool Add(T key, byte[] obj, DateTime expireTime);
        /// <summary>
        ///     获取具有指定唯一标识的缓存数据
        /// </summary>
        /// <param name="key">缓存唯一标识</param>
        /// <returns>返回缓存数据</returns>
        byte[] Get(T key);
        /// <summary>
        ///     检查当前是否存在具有指定唯一标识的缓存
        /// </summary>
        /// <param name="key">缓存唯一标识</param>
        /// <returns>返回是否存在的标识</returns>
        bool IsExists(T key);
        /// <summary>
        ///     移除具有指定唯一标示的缓存
        /// </summary>
        /// <param name="key">缓存唯一标识</param>
        void Remove(T key);
        /// <summary>
        ///     过期事件
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<T>> Expired;
    }
}