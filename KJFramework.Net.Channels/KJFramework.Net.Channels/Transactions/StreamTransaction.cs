using KJFramework.Basic.Enum;
using KJFramework.Statistics;
using KJFramework.Tracing;
using System;
using System.Collections.Generic;
using System.IO;

namespace KJFramework.Net.Channels.Transactions
{
    /// <summary>
    ///     流事物抽象父类，提供了相关的基本操作。
    /// </summary>
    /// <typeparam name="TStream">流类型</typeparam>
    public abstract class StreamTransaction<TStream> : IStreamTransaction<TStream>
        where TStream : Stream
    {
        #region 构造函数

        /// <summary>
        ///     流事物抽象父类，提供了相关的基本操作。
        /// </summary>
        /// <param name="stream">流</param>
        protected StreamTransaction(TStream stream) : this(stream, false)
        {
        }

        /// <summary>
        ///     流事物抽象父类，提供了相关的基本操作。
        /// </summary>
        /// <param name="stream">流</param>
        /// <param name="canAsync">异步标示</param>
        protected StreamTransaction(TStream stream, bool canAsync)
        {
            if (stream == null)
            {
                throw new System.Exception("无法初始化流事物，因为给定了非法的流。");
            }
            _stream = stream;
            _canAsync = canAsync;
            if (_canAsync)
            {
                ProcAsync();
            }
            else
            {
                Proc();
            }
        }

        #endregion

        #region 成员

        protected bool _canAsync;
        protected bool _enable = true;
        protected TStream _stream;
        protected Dictionary<StatisticTypes, IStatistic> _statistics = new Dictionary<StatisticTypes,IStatistic>();
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof (StreamTransaction<TStream>));
        protected Action<byte[]> _callback;

        #endregion

        #region 方法

        /// <summary>
        ///     内部执行
        /// </summary>
        protected void Proc()
        {
            try
            {
                InnerProc();
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                EndWork();
            }
        }

        /// <summary>
        ///     停止工作
        /// </summary>
        private void EndWork()
        {
            try
            {
                _enable = false;
                InnerEndWork();
            }
            catch (System.Exception e)
            {
                _tracing.Error(e, null);
            }
        }

        /// <summary>
        ///     内部执行
        /// </summary>
        protected abstract void InnerProc();
        /// <summary>
        ///     开始工作
        ///     <para>* 此方法在事物开始工作的时候将会被调用。</para>
        /// </summary>
        protected abstract void BeginWork();
        /// <summary>
        ///     停止工作
        ///     <para>* 此方法在事物异常或者结束工作的时候将会被调用。</para>
        /// </summary>
        protected abstract void InnerEndWork();

        /// <summary>
        ///     异步执行
        /// </summary>
        protected void ProcAsync()
        {
            Action action = Proc;
            action.BeginInvoke(action.EndInvoke, action);
        }

        #endregion

        #region IStreamTransaction

        /// <summary>
        ///     获取一个值，该值标示了当前流事物的状态
        /// </summary>
        public bool Enable
        {
            get { return _enable; }
        }

        /// <summary>
        ///     获取或设置一个值，该值表示了当前事物是否可以异步执行
        /// </summary>
        public bool CanAsync
        {
            get
            {
                return _canAsync;
            }
            set
            {
                _canAsync = value;
            }
        }

        /// <summary>
        ///     获取内部流
        /// </summary>
        public TStream Stream
        {
            get { return _stream; }
        }

        /// <summary>
        ///     注册回调
        /// </summary>
        /// <param name="action">回调</param>
        public StreamTransaction<TStream> RegistCallback(Action<byte[]> action)
        {
            _callback = action;
            return this;
        }

        public event EventHandler Disconnected;
        /// <summary>
        ///     断开事件
        /// </summary>
        /// <param name="e"></param>
        protected void DisconnectedHandler(System.EventArgs e)
        {
            EventHandler handler = Disconnected;
            if (handler != null) handler(this, e);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public Dictionary<StatisticTypes, IStatistic> Statistics
        {
            get
            {
                return _statistics;
            }
            set
            {
                _statistics = value;
            }
        }

        #endregion
    }
}
