namespace KJFramework.ServiceModel.Enums
{
    /// <summary>
    ///     服务处理结果枚举
    /// </summary>
    public enum ServiceProcessResult : byte
    {
        /// <summary>
        ///     成功
        /// </summary>
        Success,
        /// <summary>
        ///     错误
        /// </summary>
        Error,
        /// <summary>
        ///     异常
        /// </summary>
        Exception,
        /// <summary>
        ///     超时
        /// </summary>
        Timeout,
        /// <summary>
        ///     未定义的
        /// </summary>
        UnDefinded
    }
}