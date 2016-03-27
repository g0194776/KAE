namespace KJFramework.Net.Cloud.Enums
{
    /// <summary>
    ///     运行时类型，用于创建网络节点的条件
    /// </summary>
    public enum RuntimeTypes
    {
        /// <summary>
        ///     本地独自运行
        ///     <para>* 此枚举值更多的将会适用于分离的网络环境，意指属于本地的自己调试，不掺杂其他服务和设备</para>
        /// </summary>
        LocalAlone,
        /// <summary>
        ///     本地运行
        ///     <para>* 此枚举值更多的将会适用于本地运行调试</para>
        /// </summary>
        Local,
        /// <summary>
        ///     功能环境运行
        /// </summary>
        Func,
        /// <summary>
        ///     性能环境运行
        /// </summary>
        Performance,
        /// <summary>
        ///     生产环境运行
        /// </summary>
        Product,
        /// <summary>
        ///     厂商环境运行
        /// </summary>
        Manufacturers
    }
}