using KJFramework.EventArgs;
using KJFramework.Net.Cloud.Tasks;
using KJFramework.Net.Exception;
using KJFramework.Tracing;
using System;
using System.Collections.Concurrent;

namespace KJFramework.Net.Cloud.Pools
{
    /// <summary>
    ///     请求任务池，提供了相关的基本操作
    /// </summary>
    /// <typeparam name="T">协议栈中父类消息类型。</typeparam>
    public class RequestTaskPool<T> : IRequestTaskPool<T>
    {
        #region 构造函数

        /// <summary>
        ///     请求任务池，提供了相关的基本操作
        /// </summary>
        /// <param name="timeoutEvent">超时事件</param>
        /// <param name="successEvent">成功事件</param>
        /// <param name="failEvent">失败事件</param>
        /// <param name="maxCount">
        ///     最大任务数
        ///     <para>* 默认值为: 30000</para>
        /// </param>
        internal RequestTaskPool(EventHandler successEvent, EventHandler failEvent, EventHandler<LightSingleArgEventArgs<T>> timeoutEvent, int maxCount = 30000)
        {
            if (maxCount <= 0) throw new System.Exception("非法的最大任务数。");
            if (successEvent == null) throw new ArgumentNullException("successEvent");
            if (failEvent == null) throw new ArgumentNullException("failEvent");
            if (timeoutEvent == null) throw new ArgumentNullException("timeoutEvent");
            _successEvent = successEvent;
            _failEvent = failEvent;
            _timeoutEvent = timeoutEvent;
            _maxCount = maxCount;
        }

        #endregion

        #region Members

        private int _maxCount;
        private readonly EventHandler _successEvent;
        private readonly EventHandler _failEvent;
        private readonly EventHandler<LightSingleArgEventArgs<T>> _timeoutEvent;
        private ConcurrentQueue<IRequestTask<T>> _tasks = new ConcurrentQueue<IRequestTask<T>>();
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(RequestTaskPool<T>));

        #endregion

        #region Implementation of IRequestTaskPool<T>

        /// <summary>
        ///     获取或设置当前所支持的最大任务数量
        /// </summary>
        public int MaxCount
        {
            get { return _maxCount; }
            set { _maxCount = value; }
        }

        /// <summary>
        ///     初始化
        /// </summary>
        /// <exception cref="InitializeFailedException">初始化失败</exception>
        public void Initialzie()
        {
            if (_tasks.Count > 0) _tasks = new ConcurrentQueue<IRequestTask<T>>();
            for (int i = 0; i < _maxCount; i++)
            {
                IRequestTask<T> task = new RequestTask<T>();
                task.ExecuteSuccessful += _successEvent;
                task.ExecuteFail += _failEvent;
                task.ExecuteTimeout += _timeoutEvent;
                _tasks.Enqueue(task);
            }
        }

        /// <summary>
        ///     租一个请求任务
        /// </summary>
        /// <returns>
        ///     返回请求任务
        /// </returns>
        public IRequestTask<T> Rent()
        {
            try
            {
                IRequestTask<T> task;
                if (_tasks.TryDequeue(out task))
                {
                    task.HasRented = true;
                    return task;
                }
                return null;
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                return null;
            }
        }

        /// <summary>
        ///     归还一个请求任务
        /// </summary>
        /// <param name="task">任务</param>
        public void Giveback(IRequestTask<T> task)
        {
            task.Reset();
            _tasks.Enqueue(task);
        }

        #endregion
    }
}