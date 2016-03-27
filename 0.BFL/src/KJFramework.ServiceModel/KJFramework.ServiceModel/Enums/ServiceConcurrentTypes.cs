namespace KJFramework.ServiceModel.Enums
{
    /// <summary>
    ///     服务实例类型枚举
    ///     <para>* 此枚举类型决定着服务运行时的并发效果。</para>
    /// </summary>
    public enum ServiceConcurrentTypes
    {
        /// <summary>
        ///     单一实例
        /// </summary>
        Singleton,
        /// <summary>
        ///     多线程并发
        /// </summary>
        Multi,
        /// <summary>
        ///     多核并发
        /// </summary>
        Concurrent,
    }
}