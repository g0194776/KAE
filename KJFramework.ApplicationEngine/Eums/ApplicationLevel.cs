namespace KJFramework.ApplicationEngine.Eums
{
    /// <summary>
    ///     KAE应用等级枚举
    /// </summary>
    public enum ApplicationLevel : byte
    {
        /// <summary>
        ///     稳定版本
        /// </summary>
        Stable = 0x00,
        /// <summary>
        ///     灰度升级的第一个版本
        /// </summary>
        Alpha = 0x01,
        /// <summary>
        ///     灰度升级的第二个版本
        /// </summary>
        Beta = 0x02,
        /// <summary>
        ///     专为数据收集工作而准备的版本
        /// </summary>
        Collector = 0x03,
        /// <summary>
        ///     调试版本
        /// </summary>
        Debug = 0x04,
        /// <summary>
        ///     第一个应对市场的并行功能版本
        /// </summary>
        F1 = 0x05,
        /// <summary>
        ///     第二个应对市场的并行功能版本
        /// </summary>
        F2 = 0x06,
        /// <summary>
        ///     第三个应对市场的并行功能版本
        /// </summary>
        F3 = 0x07,
        /// <summary>
        ///     第四个应对市场的并行功能版本
        /// </summary>
        F4 = 0x08,
        /// <summary>
        ///     第五个应对市场的并行功能版本
        /// </summary>
        F5= 0x09,
        /// <summary>
        ///    未知
        /// </summary>
        Unknown = 0x0A
    }
}