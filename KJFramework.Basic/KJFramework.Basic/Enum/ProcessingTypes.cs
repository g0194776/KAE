namespace KJFramework.Basic.Enum
{
    /// <summary>
    ///     信息状态枚举
    /// </summary>
    public enum ProcessingTypes
    {
        /// <summary>
        ///     成功
        /// </summary>
        Success,
        /// <summary>
        ///     正在加载
        /// </summary>
        Loading, 
        /// <summary>
        ///     失败
        /// </summary>
        Failed,
        /// <summary>
        ///     等待
        /// </summary>
        Wait,
        /// <summary>
        ///     不等待
        /// </summary>
        DoNotWait
    }
}