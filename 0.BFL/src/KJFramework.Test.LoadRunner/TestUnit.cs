using KJFramework.Tracing;

namespace KJFramework.Test.LoadRunner
{
    /// <summary>
    ///     ���Ե�Ԫ
    /// </summary>
    public abstract class TestUnit<TContext> : ITestUnit<TContext>
        where TContext : ITestContext
    {
        #region Implementation of ITestUnit

        protected ITestUnit<TContext> _next;
        protected readonly ITestInfo _info = new TestInfo();
        protected static readonly ITracing _tracing = TracingManager.GetTracing(typeof(TestUnit<TContext>));

        /// <summary>
        ///     ��ȡ������Ϣ
        /// </summary>
        public ITestInfo Info
        {
            get { return _info; }
        }

        /// <summary>
        ///     ��ȡ��������һ��Ҫִ�еĲ��Ե�Ԫ
        /// </summary>
        public ITestUnit<TContext> Next
        {
            get { return _next; }
            set { _next = value; }
        }

        /// <summary>
        ///     ִ�е�ǰ���Ե�Ԫ
        /// </summary>
        /// <param name="context">����������</param>
        /// <returns>����ִ�н��</returns>
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
        ///     ִ�е�ǰ���Ե�Ԫ
        /// </summary>
        /// <param name="context">����������</param>
        /// <returns>����ִ�н��</returns>
        protected abstract bool InnerExecute(TContext context);

        #endregion
    }
}