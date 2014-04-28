namespace KJFramework.ApplicationEngine.Eums
{
    /// <summary>
    ///     KAE宿主状态
    /// </summary>
    public enum KAEHostStatus : byte
    {
        /// <summary>
        ///    未知
        /// </summary>
        Unknown = 0x00,
        /// <summary>
        ///    已开启
        /// </summary>
        Started = 0x01,
        /// <summary>
        ///    已停止
        /// </summary>
        Stopped = 0x02,
        /// <summary>
        ///    异常状态
        /// </summary>
        Error = 0x03
    }
}