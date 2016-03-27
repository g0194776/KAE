using KJFramework.Enums;

namespace KJFramework.Results
{
    /// <summary>
    ///     失败的调用结果
    /// </summary>
    public sealed class FailExecuteResult<T> : ExecuteResult<T>
    {
        #region Constructor

        /// <summary>
        ///     失败的调用结果
        /// </summary>
        /// <param name="errorId">系统内部错误编号</param>
        /// <param name="reason">失败的原因</param>
        internal FailExecuteResult(byte errorId, string reason = null)
            : base(ExecuteResults.Failed, default(T), reason)
        {
            _errorId = errorId;
        }

        #endregion
    }
}