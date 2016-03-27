using KJFramework.Enums;

namespace KJFramework.Results
{
    /// <summary>
    ///     执行结果
    /// </summary>
    public class ExecuteResult : IExecuteResult
    {
        #region Constructor

        /// <summary>
        ///     执行结果
        /// </summary>
        /// <param name="state">执行结果的状态</param>
        /// <param name="resultObj">结果对象</param>
        /// <param name="error">错误信息 </param>
        public ExecuteResult(ExecuteResults state, object resultObj, string error = null)
        {
            _resultObj = resultObj;
            Error = error;
            State = state;
        }

        #endregion

        #region Members

        protected byte _errorId;
        private readonly object _resultObj;
        private static readonly IExecuteResult _failedResult = new FailExecuteResult(255);
        /// <summary>
        ///     获取执行结果的状态
        /// </summary>
        public ExecuteResults State { get; internal set; }

        /// <summary>
        ///     获取内部系统的错误编号
        /// </summary>
        public byte ErrorId
        {
            get { return _errorId; }
        }

        /// <summary>
        ///     获取错误信息
        /// </summary>
        public string Error { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        ///     获取内部所包含的调用结果对象
        /// </summary>
        /// <typeparam name="T">调用结果对象的类型</typeparam>
        /// <returns>返回调用结果</returns>
        public T GetResult<T>()
        {
            return (T) _resultObj;
        }

        /// <summary>
        ///     创造一个新的执行结果
        /// </summary>
        /// <param name="state">执行结果的状态</param>
        /// <param name="resultObj">结果对象</param>
        /// <returns>返回创建后的执行结果对象</returns>
        public static IExecuteResult Create(ExecuteResults state, object resultObj)
        {
            return new ExecuteResult(state, resultObj);
        }

        /// <summary>
        ///     创造一个新的成功执行结果
        /// </summary>
        /// <param name="resultObj">结果对象</param>
        /// <returns>返回创建后的成功执行结果对象</returns>
        public static IExecuteResult Succeed(object resultObj)
        {
            return new ExecuteResult(ExecuteResults.Succeed, resultObj);
        }

        /// <summary>
        ///     创造一个失败的执行结果
        /// </summary>
        /// <returns>返回失败的执行结果对象</returns>
        public static IExecuteResult Fail()
        {
            return _failedResult;
        }

        /// <summary>
        ///     创造一个失败的执行结果
        /// </summary>
        /// <exception cref="errorId">内部系统错误编号</exception>
        /// <exception cref="reason">失败原因</exception>
        /// <returns>返回失败的执行结果对象</returns>
        public static IExecuteResult Fail(byte errorId, string reason)
        {
            return new FailExecuteResult(errorId, reason);
        }

        #endregion
    }
}