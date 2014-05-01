using System;
using KJFramework.Net.Transaction;
using KJFramework.Tracing;

namespace KJFramework.ApplicationEngine.Processors
{
    /// <summary>
    ///    KAE消息处理器
    /// </summary>
    public abstract class KAEProcessor<T> : IKAEProcessor<T>
    {
        #region Constructor

        /// <summary>
        ///    KAE消息处理器
        /// </summary>
        /// <param name="application">当前处理器所归属的KAE应用实例</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        protected KAEProcessor(IApplication application)
        {
            if (application == null) throw new ArgumentNullException("application");
            _application = application;
        }

        #endregion

        #region Members.

        private readonly IApplication _application;
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof (KAEProcessor<T>));

        #endregion

        #region Methods.

        /// <summary>
        ///    处理一个网络请求
        /// </summary>
        /// <param name="package">消息事务</param>
        public void Process(IMessageTransaction<T> package)
        {
            try { InnerProcess(package); }
            catch (System.Exception ex) { _tracing.Error(ex); }
        }

        /// <summary>
        ///    获取当前处理器所归属的KAE应用实例
        /// </summary>
        /// <returns>返回KAE应用实例</returns>
        protected IApplication GetCurrentApplication()
        {
            return _application;
        }

        /// <summary>
        ///    处理一个网络请求
        /// </summary>
        /// <param name="package">消息事务</param>
        protected abstract void InnerProcess(IMessageTransaction<T> package);

        #endregion
    }
}