using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using KJFramework.Logger;
using KJFramework.ServiceModel.Objects;
using KJFramework.ServiceModel.Proxy;

namespace KJFramework.ServiceModel.Bussiness.Default.Async
{
    /// <summary>
    ///     异步方法代理器，提供了相关的基本操作。
    /// </summary>
    public class AsyncMethodDelegator : IAsyncMethodDelegator
    {
        #region 构造函数

        /// <summary>
        ///     异步方法代理器，提供了相关的基本操作。
        /// </summary>
        /// <param name="asyncMethodName">异步方法名</param>
        public AsyncMethodDelegator(string asyncMethodName)
        {
            _asyncMethodName = asyncMethodName;
            _lastUpdateTime = DateTime.Now;
        }

        #endregion

        #region Members

        protected ConcurrentDictionary<int, ConcurrentQueue<AsyncMethodCallback>> _tempCallbacks = new ConcurrentDictionary<int, ConcurrentQueue<AsyncMethodCallback>>();
        protected Dictionary<int, AsyncMethodCallback> _callbacks = new Dictionary<int, AsyncMethodCallback>();
        private object _callbackLockObj = new object();

        #endregion

        #region Implementation of IAsyncMethodDelegator

        private string _asyncMethodName;

        /// <summary>
        ///     获取此代理器绑定到的异步方法名称
        /// </summary>
        public string AsyncMethodName
        {
            get { return _asyncMethodName; }
        }

        /// <summary>
        ///     添加一个临时代理
        /// </summary>
        /// <param name="callback">回调函数</param>
        public void AddDelegate(AsyncMethodCallback callback)
        {
            UpdateTime();
            int threadId = Thread.CurrentThread.ManagedThreadId;
            if (_tempCallbacks.ContainsKey(threadId))
            {
                _tempCallbacks[threadId].Enqueue(callback);
                return;
            }
            ConcurrentQueue<AsyncMethodCallback> callbackQueue = new ConcurrentQueue<AsyncMethodCallback>();
            if (_tempCallbacks.TryAdd(threadId, callbackQueue))
            {
                callbackQueue.Enqueue(callback);
                return;
            }
            throw new System.Exception("无法添加一个新的临时Callback !");
        }

        /// <summary>
        ///     根据会话Id，获取指定的回调函数
        /// </summary>
        /// <param name="sessionId">会话Id</param>
        /// <returns>返回回调函数</returns>
        public AsyncMethodCallback GetDelegate(int sessionId)
        {
            UpdateTime();
            AsyncMethodCallback removeCallback;
            lock (_callbackLockObj)
            {
                if(_callbacks.TryGetValue(sessionId, out removeCallback))
                {
                    _callbacks.Remove(sessionId);
                    return removeCallback;
                }
            }
            Logs.Logger.Log("通过会话Id获取一个回调函数失败，无法从预定的集合中移除元素！");
            return null;
        }

        /// <summary>
        ///     将一个回调函数绑定到一个指定的会话Id上
        ///     <para>* 注意：使用此函数应该与调用AddDelegate函数在同一个线程上。</para>
        /// </summary>
        /// <param name="sessionId">会话Id</param>
        /// <returns>返回绑定的结果</returns>
        public bool Bind(int sessionId)
        {
            UpdateTime();
            int threadId = Thread.CurrentThread.ManagedThreadId;
            //获取当前进程所拥有的临时Callback队列
            ConcurrentQueue<AsyncMethodCallback> temps;
            if (!_tempCallbacks.TryGetValue(threadId, out temps))
            {
                return false;
            }
            //获取到了队列，却是空的
            if (temps.IsEmpty)
            {
                //非法操作
                Logs.Logger.Log("无法完成将一个回调函数绑定到一个指定的会话码操作， 因为队列是空的！");
                return false;
            }
            AsyncMethodCallback callback;
            //出队列不成功
            if (!temps.TryDequeue(out callback))
            {
                Logs.Logger.Log("无法完成将一个回调函数绑定到一个指定的会话码操作， 因为出队列不成功！");
                return false;
            }
            lock (_callbackLockObj) _callbacks.Add(sessionId, callback);
            return true;
        }

        /// <summary>
        ///     抛弃所有内部等待的回调函数
        ///     <para>* 掉用此方法后，内部将会指定所有存储的回调函数，并使用超时来当做失败的原因。</para>
        /// </summary>
        public void DiscardAllOperationForTimeout()
        {
            lock (_callbackLockObj)
            {
                foreach (KeyValuePair<int, AsyncMethodCallback> asyncMethodCallback in _callbacks)
                {
                    asyncMethodCallback.Value(new AsyncCallResult(false, new TimeoutException(String.Format("Can not complete a async operation, because this async method already timeout ! [Details]\r\nSession Id = {0}", asyncMethodCallback.Key))));
                }
                _callbacks.Clear();
            }
        }

        protected DateTime _lastUpdateTime;
        /// <summary>
        ///     获取最后一次更新的时间
        /// </summary>
        public DateTime LastUpdateTime
        {
            get { return _lastUpdateTime; }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     更新时间
        /// </summary>
        private void UpdateTime()
        {
            _lastUpdateTime = DateTime.Now;
        }

        #endregion
    }
}