namespace KJFramework.Cache.Cores
{
    /// <summary>
    ///     缓存存根元接口，提供了相关的基本操作
    /// </summary>
    /// <typeparam name="T">缓存类型</typeparam>
    public interface ICacheStub<T>
    {
        /// <summary>
        ///     获取或设置一个值，该值表示了当前缓存存根是否表示为一种固态的缓存状态
        /// </summary>
        bool Fixed { get; set; }
        /// <summary>
        ///     获取或设置缓存项
        /// </summary>
        ICacheItem<T> Cache { get; set; }
        /// <summary>
        ///     获取缓存生命周期
        /// </summary>
        /// <returns></returns>
        ICacheLease GetLease();
    }
}