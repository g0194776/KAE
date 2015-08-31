using System;
using System.IO;
using System.IO.Pipes;
using KJFramework.Cores;
using KJFramework.Net.Caches;
using KJFramework.Tracing;

namespace KJFramework.Net.Transactions
{
    /// <summary>
    ///     Pipe流事物抽象父类，提供了相关的基本操作
    /// </summary>
    public class PipeStreamTransaction : StreamTransaction<PipeStream>
    {
        #region Constructor

        /// <summary>
        ///     流事物抽象父类，提供了相关的基本操作。
        /// </summary>
        /// <param name="stream">流</param>
        /// <param name="callback">回调函数</param>
        public PipeStreamTransaction(PipeStream stream, Action<IFixedCacheStub<BuffStub>, int> callback)
            : base(stream)
        {
            _callback = callback;
        }

        /// <summary>
        ///     流事物抽象父类，提供了相关的基本操作。
        /// </summary>
        /// <param name="stream">流</param>
        /// <param name="canAsync">异步标示</param>
        /// <param name="callback">回调函数</param>
        public PipeStreamTransaction(PipeStream stream, bool canAsync, Action<IFixedCacheStub<BuffStub>, int> callback)
            : base(stream, canAsync)
        {
            _callback = callback;
        }

        #endregion

        #region Members

        protected Action<IFixedCacheStub<BuffStub>, int> _callback;
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(PipeStreamTransaction));

        #endregion

        #region Methods

        /// <summary>
        ///     内部执行
        /// </summary>
        protected override void InnerProc()
        {
            if (!_stream.CanRead || !_stream.IsConnected) InnerEndWork();
            else
            {
                IFixedCacheStub<BuffStub> stub = ChannelConst.NamedPipeBuffPool.Rent();
                if (stub == null) throw new System.Exception("#Cannot rent an async recv io-stub for Named Pipe recv async action.");
                ChannelCounter.Instance.RateOfRentNamedPipeBufferStub.Increment();
                _stream.BeginRead(stub.Cache.Segment.Segment.Array, stub.Cache.Segment.Segment.Offset, stub.Cache.Segment.Segment.Count, AsyncCallback, stub);
            }
        }

        /// <summary>
        ///     开始工作
        ///     <para>* 此方法在事物开始工作的时候将会被调用。</para>
        /// </summary>
        protected override void BeginWork()
        {
        }

        /// <summary>
        ///     停止工作
        ///     <para>* 此方法在事物异常或者结束工作的时候将会被调用。</para>
        /// </summary>
        protected override void InnerEndWork()
        {
            InnerEndWork(null);
        }

        /// <summary>
        ///    内部停止工作方法
        /// </summary>
        /// <param name="stub">缓冲区缓存存根</param>
        protected void InnerEndWork(IFixedCacheStub<BuffStub> stub)
        {
            if (_stream != null && _stream.IsConnected)
            {
                if (_stream is NamedPipeServerStream) ((NamedPipeServerStream)_stream).Disconnect();
                else _stream.Close();
            }
            if (stub != null) ChannelConst.NamedPipeBuffPool.Giveback(stub);
            _enable = false;
            DisconnectedHandler(null);
        }

        //async callback proc.
        private void AsyncCallback(IAsyncResult result)
        {
            try
            {
                int length = _stream.EndRead(result);
                //current pipe channel has been closed.
                if (length == 0) InnerEndWork((IFixedCacheStub<BuffStub>)result.AsyncState);
                else
                {
                    IFixedCacheStub<BuffStub> stub = (IFixedCacheStub<BuffStub>)result.AsyncState;
                    _callback(stub, length);
                }
            }
            //cannot read datas from this channel any more!
            catch (IOException ex)
            {
                _tracing.Error(ex, null);
                InnerEndWork((IFixedCacheStub<BuffStub>) result.AsyncState);
                return;
            }
            catch (System.Exception ex) { _tracing.Error(ex, null); }
            try { InnerProc(); }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                InnerEndWork();
            }
        }

        #endregion
    }
}
