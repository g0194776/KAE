namespace KJFramework.Basic.Enum
{
    /// <summary>
    ///     性能类型
    /// </summary>
    public enum PerformanceTypes
    {
        /// <summary>
        ///     CPU
        /// </summary>
        /// <remarks>
        ///     一般表示了CPU的使用率
        /// </remarks>
        Cpu,
        /// <summary>
        ///     内存
        /// </summary>
        /// <remarks>
        ///     一般表示了内存的使用率
        /// </remarks>
        Memory,
        /// <summary>
        ///     页面内存
        /// </summary>
        /// <remarks>
        ///     一般表示了页面内存的使用率
        /// </remarks>
        PageMemory,
        /// <summary>
        ///     温度
        /// </summary>
        /// <remarks>
        ///     一般表示了某个硬件温度
        /// </remarks>
        Temperature,
        /// <summary>
        ///     网络
        /// </summary>
        /// <remarks>
        ///     一般表示了对于网络某指标的测试
        /// </remarks>
        Network
    }
}
