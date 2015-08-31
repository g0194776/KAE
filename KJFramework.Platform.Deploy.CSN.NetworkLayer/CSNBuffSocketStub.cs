using KJFramework.Tracing;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using KJFramework.Cores;
using KJFramework.Net;
using KJFramework.Objects;

namespace KJFramework.Platform.Deploy.CSN.NetworkLayer
{
    /// <summary>
    ///     套接字异步对象存根
    ///     <para>* 此类型存根将会持有一个内存缓冲区</para>
    /// </summary>
    public class CSNBuffSocketStub : IClearable
    {
        #region Constructor

        /// <summary>
        ///     套接字IO对象数据存根
        /// </summary>
        public CSNBuffSocketStub()
        {
            _segment = CSNGlobalMemory.SegmentContainer.Rent();
            if (_segment == null) throw new System.Exception("[BuffSocketStub-Prealloc] #There has no enough MemorySegment can be used.");
            CSNChannelCounter.Instance.RateOfRentMemSegment.Increment();
            _target = new SocketAsyncEventArgs();
            _target.BufferList = new List<ArraySegment<byte>> { _segment.Segment };
            _target.Completed += Completed;
        }

        #endregion

        #region Members

        private readonly SocketAsyncEventArgs _target;
        private readonly IMemorySegment _segment;
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(CSNBuffSocketStub));

        /// <summary>
        ///     获取缓存目标
        /// </summary>
        public SocketAsyncEventArgs Target
        {
            get { return _target; }
        }

        /// <summary>
        ///     获取或设置附属属性
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        ///     获取内部关联的内存数据片段
        /// </summary>
        public IMemorySegment Segment
        {
            get { return _segment; }
        }

        #endregion

        #region Implementation of IClearable

        /// <summary>
        ///     清理资源
        /// </summary>
        public void Clear()
        {
            Tag = null;
            _target.AcceptSocket = null;
            _target.UserToken = null;
            _target.RemoteEndPoint = null;
            _segment.UsedBytes = 0;
            SocketHelper.Clear(_target);
        }

        #endregion

        #region Events

        void Completed(object sender, SocketAsyncEventArgs e)
        {
            IFixedCacheStub<CSNBuffSocketStub> stub;
            switch (e.LastOperation)
            {
                //send complated event.
                case SocketAsyncOperation.Send:
                    #region Send Completed.

                    stub = (IFixedCacheStub<CSNBuffSocketStub>)e.UserToken;
                    CSNTcpTransportChannel channel = (CSNTcpTransportChannel)stub.Tag;
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
                        CSNChannelConst.BuffAsyncStubPool.Giveback(stub);
                        channel.Disconnect();
                    }

                    #endregion
                    break;
                //send complated event.
                case SocketAsyncOperation.Receive:
                    #region Recv Completed.

                    stub = (IFixedCacheStub<CSNBuffSocketStub>)e.UserToken;
                    CSNTcpAsynDataRecevier recevier = (CSNTcpAsynDataRecevier)stub.Tag;
                    try { recevier.ProcessReceive(stub); }
                    catch (System.Exception ex) { _tracing.Error(ex, null); }

                    #endregion
                    break;
            }
        }

        #endregion
    }
}
