using System;
using System.Diagnostics;
using KJFramework.EventArgs;

namespace KJFramework.Engin.Worker
{
    /// <summary>
    ///     抽象的工作者，对一些必要的方法提供了实现。
    /// </summary>
    /// <typeparam name="T">宿主类型</typeparam>
    public abstract class Worker<T> : IWorker<T>
    {
        #region 构造函数

        /// <summary>
        ///     工作者的抽象实现
        /// </summary>
        public Worker()
        {
            _state = true;
        }

        #endregion

        #region 成员

        private delegate bool DelegateDoWork(T item);

        #endregion

        #region IWorker<T> Members

        private bool _state;
        /// <summary>
        ///     获取当前工作者的工作状态
        /// </summary>
        public bool State
        {
            get { return _state; }
        }

        /// <summary>
        ///     执行方法，使用当前线程进行方法的执行操作。
        /// </summary>
        /// <param name="item">工作宿主 [ref]</param>
        /// <returns>返回工作的状态</returns>
        public abstract bool DoWork(T item);

        /// <summary>
        ///     执行方法，异步进行方法的执行操作。
        /// </summary>
        /// <param name="item">工作宿主 [ref]</param>
        /// <returns>返回工作的状态</returns>
        public bool DoWorkAsyn(T item)
        {
            DelegateDoWork delegateDoWork = DoWork;
            try
            {
                IAsyncResult result =  delegateDoWork.BeginInvoke(item, null, null);
                return delegateDoWork.EndInvoke(result);
            }
            catch (System.Exception e)
            {
                Debug.WriteLine("异步执行操作失败 ： " + e.Message);
            }
            return false;
        }

        /// <summary>
        ///     工作者工作状态汇报事件
        /// </summary>
        public event DelegateWorkerProcessing WorkerProcessing;
        protected void WorkerProcessingHandler(WorkerProcessingEventArgs e)
        {
            DelegateWorkerProcessing processing = WorkerProcessing;
            if (processing != null) processing(this, e);
        }

        /// <summary>
        ///     工作者开始工作事件
        /// </summary>
        public event DelegateBeginWork BeginWork;
        protected void BeginWorkHandler(BeginWorkEventArgs e)
        {
            DelegateBeginWork work = BeginWork;
            if (work != null) work(this, e);
        }

        /// <summary>
        ///     工作者停止工作事件
        /// </summary>
        public event DelegateEndWork EndWork;
        protected void EndWorkHandler(EndWorkEventArgs e)
        {
            DelegateEndWork work = EndWork;
            if (work != null) work(this, e);
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}