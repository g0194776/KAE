using System;
using System.Collections.Generic;
using System.IO.Pipes;
using KJFramework.Basic.Enum;
using KJFramework.EventArgs;
using KJFramework.Logger;
using KJFramework.Net.Channels.Statistics;
using KJFramework.Net.Channels.Uri;
using KJFramework.Statistics;

namespace KJFramework.Net.Channels.HostChannels
{
    /// <summary>
    ///     命名管道宿主通讯通道，提供了相关的基本操作
    /// </summary>
    public class PipeHostTransportChannel : HostTransportChannel, IPiepHostTransportChannel
    {
        #region 构造函数

        /// <summary>
        ///     命名管道宿主通讯通道，提供了相关的基本操作
        /// </summary>
        /// <param name="name">实例名称</param>
        /// <exception cref="System.ArgumentException">参数错误</exception>
        /// <exception cref="System.ArgumentNullException">参数错误</exception>
        public PipeHostTransportChannel(String name) : this(name, 10)
        { }

        /// <summary>
        ///     命名管道宿主通讯通道，提供了相关的基本操作
        /// </summary>
        /// <param name="name">管道名称</param>
        /// <param name="instanceCount">实例数</param>
        /// <exception cref="System.ArgumentException">参数错误</exception>
        /// <exception cref="System.ArgumentNullException">参数错误</exception>
        public PipeHostTransportChannel(String name, int instanceCount)
        {
            if (instanceCount <= 0) throw new ArgumentException("Illegal instance count.");
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");
            _name = name;
            _instanceCount = instanceCount;
            #if (DEBUG)
            {
                _statistics = new Dictionary<StatisticTypes, IStatistic>();
                PipeHostTransportChannelStatistic statistic = new PipeHostTransportChannelStatistic();
                statistic.Initialize(this);
                _statistics.Add(StatisticTypes.Network, statistic);
            }
            #endif
        }

        /// <summary>
        ///     命名管道宿主通讯通道，提供了相关的基本操作
        /// </summary>
        /// <param name="uri">管道URL</param>
        /// <param name="instanceCount">实例数</param>
        /// <exception cref="System.ArgumentException">参数错误</exception>
        /// <exception cref="System.ArgumentNullException">参数错误</exception>
        public PipeHostTransportChannel(PipeUri uri, int instanceCount)
        {
            if (instanceCount <= 0) throw new System.Exception("Illegal instance count.");
            if (uri == null) throw new ArgumentNullException("uri");
            LogicalAddress = uri;
            _name = uri.PipeName;
            _instanceCount = instanceCount;
            #if (DEBUG)
            {
                _statistics = new Dictionary<StatisticTypes, IStatistic>();
                PipeHostTransportChannelStatistic statistic = new PipeHostTransportChannelStatistic();
                statistic.Initialize(this);
                _statistics.Add(StatisticTypes.Network, statistic);
            }
            #endif
        }

        #endregion

        #region Members

        protected readonly string _name;
        protected readonly int _instanceCount;
        protected NamedPipeServerStream[] _pipeStreams;

        #endregion

        #region Overrides of HostTransportChannel

        /// <summary>
        ///     注册网络
        /// </summary>
        /// <returns>返回注册的状态</returns>
        public override bool Regist()
        {
            if (_pipeStreams == null)
            {
                _pipeStreams = new NamedPipeServerStream[_instanceCount];
                for (int i = 0; i < _instanceCount; i++)
                {
                    _pipeStreams[i] = new NamedPipeServerStream(_name, PipeDirection.InOut, _instanceCount, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
                    _pipeStreams[i].BeginWaitForConnection(CallbackWaitConnection, _pipeStreams[i]);
                }
                return true;
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
                if (_pipeStreams != null)
                {
                    foreach (NamedPipeServerStream stream in _pipeStreams)
                    {
                        if (stream.IsConnected)
                        {
                            stream.Disconnect();
                        }
                        stream.Close();
                    }
                    _pipeStreams = null;
                }
                return true;
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex, DebugGrade.Standard, Logs.Name);
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
            try
            {
                NamedPipeServerStream pipeServerStream = (NamedPipeServerStream)result.AsyncState;
                pipeServerStream.EndWaitForConnection(result);
                PipeTransportChannel channel = new PipeTransportChannel(pipeServerStream);
                channel.Disconnected += TransportChannelDisconnected;
                ChannelCreatedHandler(new LightSingleArgEventArgs<ITransportChannel>(channel));
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
            }
        }
       
        #endregion

        #region 事件

        void TransportChannelDisconnected(object sender, System.EventArgs e)
        {
            PipeTransportChannel channel = (PipeTransportChannel)sender;
            channel.Disconnected -= TransportChannelDisconnected;
            ChannelDisconnectedHandler(new LightSingleArgEventArgs<ITransportChannel>(channel));
            if (channel.Stream != null)
            {
                ((NamedPipeServerStream)channel.Stream).BeginWaitForConnection(CallbackWaitConnection, channel.Stream);
            }
        }

        #endregion
    }
}