using KJFramework.IO.Helper;
using KJFramework.Tracing;
using System;
using System.IO;
using System.IO.Pipes;

namespace KJFramework.Net.Channels.Transactions
{
    /// <summary>
    ///     Pipe流事物抽象父类，提供了相关的基本操作
    /// </summary>
    public abstract class PipeStreamTransaction<TStream> : StreamTransaction<TStream>
        where TStream : PipeStream
    {
        #region 构造函数

        /// <summary>
        ///     流事物抽象父类，提供了相关的基本操作。
        /// </summary>
        /// <param name="stream">流</param>
        /// <param name="callback">回调函数</param>
        public PipeStreamTransaction(TStream stream, Action<byte[]> callback)
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
        public PipeStreamTransaction(TStream stream, bool canAsync, Action<byte[]> callback)
            : base(stream, canAsync)
        {
            _callback = callback;
        }

        #endregion

        #region Members

        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(PipeStreamTransaction<TStream>));

        #endregion

        #region 父类方法

        /// <summary>
        ///     内部执行
        /// </summary>
        protected override void InnerProc()
        {
            if (!_stream.CanRead || !_stream.IsConnected)
            {
                _enable = false;
                InnerEndWork();
                return;
            }
            byte[] buffer = new byte[ChannelConst.RecvBufferSize];
            _stream.BeginRead(buffer, 0, buffer.Length, AsyncCallback, buffer);
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
        protected abstract override void InnerEndWork();

        #endregion

        #region Methods

        //async callback proc.
        private void AsyncCallback(IAsyncResult result)
        {
            try
            {
                int length = _stream.EndRead(result);
                //current pipe channel has been closed.
                if (length == 0)
                {
                    _enable = false;
                    InnerEndWork();
                    return;
                }
                byte[] data = (byte[])result.AsyncState;
                _callback(ByteArrayHelper.GetNextData(data, 0, length));
            }
            //cannot read datas from this channel any more!
            catch(IOException ex)
            {
                _tracing.Error(ex, null);
                _enable = false;
                InnerEndWork();
                return;
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                //continue async read.
                if (_stream.CanRead && _stream.IsConnected)
                {
                    byte[] buffer = new byte[ChannelConst.RecvBufferSize];
                    _stream.BeginRead(buffer, 0, buffer.Length, AsyncCallback, buffer);
                }
                else
                {
                    _enable = false;
                    InnerEndWork();
                }
            }
            finally
            {
                if (_stream.IsConnected && _stream.CanRead)
                {
                    byte[] buffer = new byte[ChannelConst.RecvBufferSize];
                    _stream.BeginRead(buffer, 0, buffer.Length, AsyncCallback, buffer);
                }
                else
                {
                    _enable = false;
                    InnerEndWork();
                }
            }
        }

        #endregion
    }
}
