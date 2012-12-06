namespace KJFramework.Cache.Cores
{
    /// <summary>
    ///     缓存项元接口，提供了最基础的缓存提取和存储功能
    /// </summary>
    /// <typeparam name="T">缓存类型</typeparam>
    public interface ICacheItem<T>
    {
        /// <summary>
        ///     获取缓存内容
        /// </summary>
        /// <returns>返回缓存内容</returns>
        T GetValue();
        /// <summary>
        ///     设置缓存内容
        /// </summary>
        /// <param name="obj">缓存对象</param>
        void SetValue(T obj);
    }
}