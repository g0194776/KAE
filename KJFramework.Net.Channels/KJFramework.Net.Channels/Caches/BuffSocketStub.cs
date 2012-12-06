using System;
using System.Collections.Generic;
using KJFramework.Basic.Enum;
using KJFramework.Cache;
using KJFramework.Cache.Cores;
using KJFramework.Cache.Objects;
using KJFramework.Logger;
using KJFramework.Net.Channels.Receivers;
using System.Net.Sockets;

namespace KJFramework.Net.Channels.Caches
{
    /// <summary>
    ///     �׽����첽������
    ///     <para>* �����ʹ���������һ���ڴ滺����</para>
    /// </summary>
    public class BuffSocketStub : IClearable
    {
        #region Constructor

        /// <summary>
        ///     �׽���IO�������ݴ��
        /// </summary>
        public BuffSocketStub()
        {
            _segment = GlobalMemory.SegmentContainer.Rent();
            if (_segment == null) throw new System.Exception("[BuffSocketStub-Prealloc] #There has no enough MemorySegment can be used.");
            ChannelCounter.Instance.RateOfRentMemSegment.Increment();
            _target = new SocketAsyncEventArgs();
            _target.BufferList = new List<ArraySegment<byte>> {_segment.Segment};
            _target.Completed += Completed;
        }

        #endregion

        #region Members

        private readonly SocketAsyncEventArgs _target;
        private readonly IMemorySegment _segment;

        /// <summary>
        ///     ��ȡ����Ŀ��
        /// </summary>
        public SocketAsyncEventArgs Target
        {
            get { return _target; }
        }

        /// <summary>
        ///     ��ȡ�����ø�������
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        ///     ��ȡ�ڲ��������ڴ�����Ƭ��
        /// </summary>
        public IMemorySegment Segment
        {
            get { return _segment; }
        }

        #endregion

        #region Implementation of IClearable

        /// <summary>
        ///     ������Դ
        /// </summary>
        public void Clear()
        {
            Tag = null;
            _target.AcceptSocket = null;
            _target.UserToken = null;
            _target.RemoteEndPoint = null;
            SocketHelper.Clear(_target);
        }

        #endregion

        #region Events

        void Completed(object sender, SocketAsyncEventArgs e)
        {
            IFixedCacheStub<BuffSocketStub> stub;
            switch (e.LastOperation)
            {
                    //send complated event.
                    case SocketAsyncOperation.Send:
                    #region Send Completed.

                    stub = (IFixedCacheStub<BuffSocketStub>) e.UserToken;
                    TcpTransportChannel channel = (TcpTransportChannel) stub.Tag;
                    if (e.SocketError != SocketError.Success && channel.IsConnected)
                    {
                        Logs.Logger.Log(
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

                    stub = (IFixedCacheStub<BuffSocketStub>)e.UserToken;
                    TcpAsynDataRecevier recevier = (TcpAsynDataRecevier)stub.Tag;
                    try { recevier.ProcessReceive(stub.Cache.Target, stub.Cache.Segment); }
                    catch (System.Exception ex) { Logs.Logger.Log(ex, DebugGrade.High); }
                    finally { ChannelConst.BuffAsyncStubPool.Giveback(stub); }

                    #endregion
                    break;
            }
        }

        #endregion
    }
}