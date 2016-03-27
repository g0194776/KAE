using KJFramework.Tracing;

namespace KJFramework.Test.LoadRunner
{
    /// <summary>
    ///     测试单元
    /// </summary>
    public abstract class TestUnit<TContext> : ITestUnit<TContext>
        where TContext : ITestContext
    {
        #region Implementation of ITestUnit

        protected ITestUnit<TContext> _next;
        protected readonly ITestInfo _info = new TestInfo();
        protected static readonly ITracing _tracing = TracingManager.GetTracing(typeof(TestUnit<TContext>));

        /// <summary>
        ///     获取测试信息
        /// </summary>
        public ITestInfo Info
        {
            get { return _info; }
        }

        /// <summary>
        ///     获取或设置下一个要执行的测试单元
        /// </summary>
        public ITestUnit<TContext> Next
        {
            get { return _next; }
            set { _next = value; }
        }

        /// <summary>
        ///     执行当前测试单元
        /// </summary>
        /// <param name="context">测试上下文</param>
        /// <returns>返回执行结果</returns>
        public bool Execute(TContext context)
        {
            try { return InnerExecute(context); }
            catch(System.Exception ex)
            {
                _tracing.Error(ex, null);
                return false;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     执行当前测试单元
        /// </summary>
        /// <param name="context">测试上下文</param>
        /// <returns>返回执行结果</returns>
        protected abstract bool InnerExecute(TContext context);

        #endregion
    }
}