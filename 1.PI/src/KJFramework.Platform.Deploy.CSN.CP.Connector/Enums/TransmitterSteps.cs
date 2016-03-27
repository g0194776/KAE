namespace KJFramework.Platform.Deploy.CSN.CP.Connector.Enums
{
    /// <summary>
    ///     传输步骤枚举
    /// </summary>
    public enum TransmitterSteps
    {
        /// <summary>
        ///     初始化策略
        /// </summary>
        InitializePolicy,
        /// <summary>
        ///     正在通知将要进行的多包操作
        /// </summary>
        Notify,
        /// <summary>
        ///     传输开始发送的标示
        /// </summary>
        BeginTransfer,
        /// <summary>
        ///     传输数据中
        /// </summary>
        TransferData,
        /// <summary>
        ///     传输结束发送的标示
        /// </summary>
        EndTransfer,
        /// <summary>
        ///     传输一个结果，不采用分包传输
        /// </summary>
        TransferDataWithoutMultiPackage,
        /// <summary>
        ///     超时了
        /// </summary>
        Timeout,
        /// <summary>
        ///     异常情况
        /// </summary>
        Exception,
        /// <summary>
        ///     完成任务
        /// </summary>
        Finish
    }
}