using System;
using System.Collections.Generic;
using System.Net.Sockets;
using KJFramework.Cores;
using KJFramework.Net.Receivers;
using KJFramework.Tracing;

namespace KJFramework.Net.Caches
{
    /// <summary>
    ///     套接字异步对象存根
    ///     <para>* 此类型存根将会持有一个内存缓冲区</para>
    /// </summary>
    public class SocketBuffStub : BuffStub
    {
        #region Constructor

        /// <summary>
        ///     套接字IO对象数据存根
        /// </summary>
        public SocketBuffStub()
        {
            _target = new SocketAsyncEventArgs();
            _target.BufferList = new List<ArraySegment<byte>> {_segment.Segment};
            _target.Completed += Completed;
        }

        #endregion

        #region Members

        private readonly SocketAsyncEventArgs _target;
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(SocketBuffStub));

        /// <summary>
        ///     获取缓存目标
        /// </summary>
        public SocketAsyncEventArgs Target
        {
            get { return _target; }
        }

        #endregion

        #region Implementation of IClearable

        /// <summary>
        ///     清理资源
        /// </summary>
        public override void Clear()
        {
            _target.AcceptSocket = null;
            _target.UserToken = null;
            _target.RemoteEndPoint = null;
            SocketHelper.Clear(_target);
            base.Clear();
        }

        #endregion

        #region Events

        void Completed(object sender, SocketAsyncEventArgs e)
        {
            IFixedCacheStub<SocketBuffStub> stub;
            switch (e.LastOperation)
            {
                    //send complated event.
                    case SocketAsyncOperation.Send:
                    #region Send Completed.

                    stub = (IFixedCacheStub<SocketBuffStub>)e.UserToken;
                    TcpTransportChannel channel = (TcpTransportChannel) stub.Tag;
                    if (e.SocketError != SocketError.Success && channel.IsConnected)
                    {
                        _tracing.Warn(
                            string.Format(
                                "The target channel SendAsync status has incorrectly, so the framework decided to disconnect it.\r\nL: {0}\r\nR: {1}\r\nSocket-Error: {2}\r\nBytesTransferred: {3}\r\n",
                                channel.LocalEndPoint, 
                                channel.RemoteEndPoint, 
                                e.SocketError,
                                e.BytesTransferred));
                        //giveback it at first.
                        ChannelConst.BuffAsyncStubPool.Giveback(stub);
                        channel.Disconnect();
                    }

                    #endregion
                    break;
                    //send complated event.
                    case SocketAsyncOperation.Receive:
                    #region Recv Completed.

                    stub = (IFixedCacheStub<SocketBuffStub>)e.UserToken;
                    TcpAsynDataRecevier recevier = (TcpAsynDataRecevier)stub.Tag;
                    try { recevier.ProcessReceive(stub); }
                    catch (System.Exception ex) { _tracing.Error(ex, null); }

                    #endregion
                    break;
            }
        }

        #endregion
    }
}