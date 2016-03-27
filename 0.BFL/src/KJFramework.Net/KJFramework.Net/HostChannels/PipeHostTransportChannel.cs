using System;
using System.Collections.Generic;
using System.IO.Pipes;
using KJFramework.EventArgs;
using KJFramework.Net.Uri;
using KJFramework.Tracing;

namespace KJFramework.Net.HostChannels
{
    /// <summary>
    ///     命名管道宿主通讯通道，提供了相关的基本操作
    /// </summary>
    public class PipeHostTransportChannel : HostTransportChannel, IPiepHostTransportChannel
    {
        #region Constructors

        /// <summary>
        ///     命名管道宿主通讯通道，提供了相关的基本操作
        /// </summary>
        /// <param name="name">管道完整URI</param>
        /// <exception cref="System.ArgumentException">参数错误</exception>
        /// <exception cref="System.ArgumentNullException">参数错误</exception>
        public PipeHostTransportChannel(String name) :
            this(new PipeUri(name), 254)
        { }

        /// <summary>
        ///     命名管道宿主通讯通道，提供了相关的基本操作
        /// </summary>
        /// <param name="name">管道完整URI</param>
        /// <param name="instanceCount">实例数</param>
        /// <exception cref="System.ArgumentException">参数错误</exception>
        /// <exception cref="System.ArgumentNullException">参数错误</exception>
        public PipeHostTransportChannel(String name, int instanceCount)
            : this(new PipeUri(name), instanceCount)
        { }

        /// <summary>
        ///     命名管道宿主通讯通道，提供了相关的基本操作
        /// </summary>
        /// <param name="uri">管道URL</param>
        /// <param name="instanceCount">实例数</param>
        /// <exception cref="System.ArgumentException">参数错误</exception>
        /// <exception cref="System.ArgumentNullException">参数错误</exception>
        public PipeHostTransportChannel(PipeUri uri, int instanceCount)
        {
            if (uri == null) throw new ArgumentNullException("uri");
            if (instanceCount > 254 || instanceCount <= 0) throw new ArgumentException("#Illegal Named channel instance count.");
            LogicalAddress = uri;
            _name = uri.PipeName;
            _instanceCount = instanceCount;
            _uri = uri;
        }

        #endregion

        #region Members

        protected readonly string _name;
        protected readonly PipeUri _uri;
        protected readonly int _instanceCount;
        private readonly object _lockAvaObj = new object();
        private readonly object _lockUsedObj = new object();
        protected readonly IList<NamedPipeServerStream> _avaStreams = new List<NamedPipeServerStream>();
        protected readonly IList<NamedPipeServerStream> _usedStreams = new List<NamedPipeServerStream>();
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(PipeHostTransportChannel));
        /// <summary>
        ///    获取可用的命名管道个数
        /// </summary>
        public int AvailableCount { get { return _avaStreams.Count; } }
        /// <summary>
        ///    获取已使用的命名管道个数
        /// </summary>
        public int UsedCount { get { return _usedStreams.Count; } }
        /// <summary>
        ///    获取所有的命名管道个数
        /// </summary>
        public int TotalCount { get { return _instanceCount; } }

        #endregion

        #region Overrides of HostTransportChannel

        /// <summary>
        ///     注册网络
        /// </summary>
        /// <returns>返回注册的状态</returns>
        public override bool Regist()
        {
            return Regist(PipeDirection.InOut);
        }

        /// <summary>
        ///     注册网络
        /// </summary>
        /// <param name="direction">命名管道数据流向类型</param>
        /// <returns>返回注册的状态</returns>
        public virtual bool Regist(PipeDirection direction)
        {
            if(_usedStreams.Count != 0 || _avaStreams.Count != 0)
                throw new InvalidOperationException("#You cannot did Regist operation again on an initialized Host Channel.");

                for (int i = 0; i < _instanceCount; i++)
                {
                    NamedPipeServerStream stream;
                    lock (_lockAvaObj)
                    {
                         stream= new NamedPipeServerStream(_name, direction, _instanceCount, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
                         _avaStreams.Add(stream);
                    }
                    stream.BeginWaitForConnection(CallbackWaitConnection, stream);
            }
            
            return true;
        }

        /// <summary>
        ///     注销网络
        /// </summary>
        /// <returns>返回注册的状态</returns>
        public override bool UnRegist()
        {
            try
            {
                lock (_lockAvaObj)
                {
                    foreach (NamedPipeServerStream stream in _avaStreams)
                    {
                        if (stream.IsConnected) stream.Disconnect();
                        stream.Close();
                    }
                    _avaStreams.Clear();
                }
                lock (_lockUsedObj)
                {
                    foreach (NamedPipeServerStream stream in _usedStreams)
                    {
                        if (stream.IsConnected) stream.Disconnect();
                        stream.Close();
                    }
                    _usedStreams.Clear();
                }
                return true;
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                return true;
            }
        }

        #endregion

        #region Implementation of IPiepHostTransportChannel

        /// <summary>
        ///     获取命名管道的实例名称
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        ///     获取监听的管道地址
        /// </summary>
        public PipeUri LogicalAddress { get; private set; }

        /// <summary>
        ///     获取实例个数
        /// </summary>
        public int InstanceCount
        {
            get { return _instanceCount; }
        }

        #endregion

        #region Methods

        private void CallbackWaitConnection(IAsyncResult result)
        {
            NamedPipeServerStream stream = (NamedPipeServerStream)result.AsyncState;
            try
            {
                stream.EndWaitForConnection(result);
                lock (_lockAvaObj) _avaStreams.Remove(stream);
                lock (_lockUsedObj) _usedStreams.Add(stream);
                PipeTransportChannel channel = new PipeTransportChannel(_uri, stream);
                channel.Disconnected += TransportChannelDisconnected;
                //transfers current stream into different collection.
                ChannelCreatedHandler(new LightSingleArgEventArgs<ITransportChannel>(channel));
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
            }
        }
       
        #endregion

        #region Events

        void TransportChannelDisconnected(object sender, System.EventArgs e)
        {
            PipeTransportChannel channel = (PipeTransportChannel)sender;
            channel.Disconnected -= TransportChannelDisconnected;
            //transfers current stream into different collection.
            lock (_lockUsedObj) _usedStreams.Remove((NamedPipeServerStream) channel.Stream);
            lock (_lockAvaObj) _avaStreams.Add((NamedPipeServerStream) channel.Stream);
            ((NamedPipeServerStream)channel.Stream).BeginWaitForConnection(CallbackWaitConnection, channel.Stream);
            ChannelDisconnectedHandler(new LightSingleArgEventArgs<ITransportChannel>(channel));
        }

        #endregion
    }
}