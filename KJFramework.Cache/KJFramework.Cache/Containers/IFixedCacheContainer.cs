using KJFramework.Cache.Cores;

namespace KJFramework.Cache.Containers
{
    /// <summary>
    ///     固定容量的缓存容器元接口，提供了相关的基本操作
    /// </summary>
    /// <typeparam name="T">缓存类型</typeparam>
    public interface IFixedCacheContainer<T>
        where T : IClearable, new()
    {
        /// <summary>
        ///     获取当前容器的最大容量
        /// </summary>
        int Capacity { get; }
        /// <summary>
        ///     租借一个缓存
        /// </summary>
        /// <returns>返回一个新的缓存</returns>
        IFixedCacheStub<T> Rent();
        /// <summary>
        ///     归还一个缓存
        /// </summary>
        /// <param name="cache">缓存</param>
        void Giveback(IFixedCacheStub<T> cache);
        /// <summary>
        ///    构造内部性能计数器
        /// </summary>
        /// <param name="name">性能计数器名称</param>
        void BuildPerformanceCounter(string name);
    }
}