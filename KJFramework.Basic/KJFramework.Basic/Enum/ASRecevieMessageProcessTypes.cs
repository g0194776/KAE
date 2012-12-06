namespace KJFramework.Basic.Enum
{
    /// <summary>
    ///     应用服务器接收消息处理类型
    /// </summary>
    public enum ASRecevieMessageProcessTypes
    {
        /// <summary>
        ///     处理
        /// </summary>
        Process,
        /// <summary>
        ///     不处理（返回null）
        /// </summary>
        NoProcess,
        /// <summary>
        ///     返回黑名单消息
        /// </summary>
        BlackList
    }
}