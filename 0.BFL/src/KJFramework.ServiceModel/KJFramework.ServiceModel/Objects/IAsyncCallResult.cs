namespace KJFramework.ServiceModel.Objects
{
    /// <summary>
    ///     异步调用结果元接口，提供了相关的基本操作
    /// </summary>
    public interface IAsyncCallResult
    {
        /// <summary>
        ///     获取一个值，该值标示了当前的异步调用是否成功
        /// </summary>
        bool IsSuccess { get; }
        /// <summary>
        ///     获取一个值，该值标示了当前的异步调用是否包含返回值
        /// </summary>
        bool HasResult { get; }
        /// <summary>
        ///     获取最后一个错误信息
        /// </summary>
        System.Exception LastError { get; }
        /// <summary>
        ///     获取返回值
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <returns>返回值</returns>
        T GetResult<T>();
    }
}