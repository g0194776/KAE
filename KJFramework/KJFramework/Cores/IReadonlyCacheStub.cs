namespace KJFramework.Cores
{
    /// <summary>
    ///     只读缓存存根元接口，提供了相关的基本操作
    /// </summary>
    /// <typeparam name="T">缓存类型</typeparam>
    public interface IReadonlyCacheStub<T>
    {
        /// <summary>
        ///     获取缓存
        /// </summary>
        T Cache { get; }
        /// <summary>
        ///     获取缓存的生命周期
        /// </summary>
        ICacheLease Lease { get; }
    }
}