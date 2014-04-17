namespace KJFramework.ApplicationEngine.Eums
{
    /// <summary>
    ///    应用状态枚举
    /// </summary>
    public enum ApplicationStatus : byte
    {
        /// <summary>
        ///    未知
        /// </summary>
        Unknown = 0x00,
        /// <summary>
        ///    初始化中
        /// </summary>
        Initializing = 0x01,
        /// <summary>
        ///    已暂停
        /// </summary>
        Paused = 0x02,
        /// <summary>
        ///    正在运行
        /// </summary>
        Running = 0x03,
        /// <summary>
        ///    正在停止
        /// </summary>
        Stoping = 0x04,
        /// <summary>
        ///    已停止
        /// </summary>
        Stopped = 0x05,
        /// <summary>
        ///    正在升级
        /// </summary>
        Upgrading = 0x06,
        /// <summary>
        ///    已初始化完成
        /// </summary>
        Initialized = 0x07,
        /// <summary>
        ///     异常
        /// </summary>
        Exception = 0x08
    }
}