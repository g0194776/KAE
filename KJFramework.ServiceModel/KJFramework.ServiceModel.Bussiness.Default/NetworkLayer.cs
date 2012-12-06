using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using KJFramework.EventArgs;
using KJFramework.Logger;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.HostChannels;
using KJFramework.ServiceModel.Bussiness.Default.Centers;
using KJFramework.ServiceModel.Bussiness.Default.Counters;
using KJFramework.ServiceModel.Bussiness.Default.Messages;
using KJFramework.ServiceModel.Bussiness.Default.Services;
using KJFramework.ServiceModel.Bussiness.Default.Transactions;
using KJFramework.Tracing;

namespace KJFramework.ServiceModel.Bussiness.Default
{
    /// <summary>
    ///     服务模型的网络层，负责对接客户与服务的网络需求
    /// </summary>
    internal class NetworkLayer
    {
        #region Constructor

        private NetworkLayer()
        {
        }

        #endregion

        #region Members

        /// <summary>
        ///     服务模型的网络层，负责对接客户与服务的网络需求
        /// </summary>
        public static readonly NetworkLayer Instance = new NetworkLayer();
        private ConcurrentDictionary<String, IHostTransportChannel> _pipeHostChannels = new ConcurrentDictionary<String, IHostTransportChannel>();
        private ConcurrentDictionary<int, IHostTransportChannel> _tcpHostChannels = new ConcurrentDictionary<int, IHostTransportChannel>();
        private ConcurrentDictionary<Guid, IMessageTransportChannel<Message>> _transportChannels = new ConcurrentDictionary<Guid, IMessageTransportChannel<Message>>();
        private ITracing _tracing = TracingManager.GetTracing(typeof(NetworkLayer));
        #endregion

        #region Functions

        /// <summary>
        ///     申请一个宿主通道
        /// </summary>
        /// <param name="channel">宿主通道</param>
        /// <returns>返回申请的结果</returns>
        public bool Alloc(IHostTransportChannel channel)
        {
            if (channel == null) return false;
            if (channel is TcpHostTransportChannel) return TcpAlloc((TcpHostTransportChannel)channel);
            if (channel is PipeHostTransportChannel) return PipeAlloc((PipeHostTransportChannel)channel);
            throw new System.Exception("不被支持的宿主通道类型。");
        }

        /// <summary>
        ///     获取具有指定唯一KEY的传输通道
        /// </summary>
        /// <param name="key">通道KEY</param>
        /// <returns>返回传输通道</returns>
        public IMessageTransportChannel<Message> GetTransportChannel(Guid key)
        {
            IMessageTransportChannel<Message> channel;
            return _transportChannels.TryGetValue(key, out channel) ? channel : null;
        }

        /// <summary>
        ///     移除一个具有指定唯一KEY的宿主通道
        /// </summary>
        /// <param name="port">TCP端口</param>
        public void RemoveTcpHostChannel(int port)
        {
            if (port <= 0) return;
            if (!_tcpHostChannels.ContainsKey(port)) return;
            IHostTransportChannel hostTransportChannel;
            _tcpHostChannels[port].UnRegist();
            _tcpHostChannels.TryRemove(port, out hostTransportChannel);
            hostTransportChannel.ChannelCreated -= ChannelCreated;
            hostTransportChannel.ChannelDisconnected -= ChannelDisconnected;
        }

        /// <summary>
        ///     移除一个具有指定唯一KEY的宿主通道
        /// </summary>
        /// <param name="uri">IPC逻辑地址</param>
        public void RemoveIpcHostChannel(String uri)
        {
            if (String.IsNullOrEmpty(uri)) return;
            if (!_pipeHostChannels.ContainsKey(uri)) return;
            IHostTransportChannel hostTransportChannel;
            _pipeHostChannels[uri].UnRegist();
            _pipeHostChannels.TryRemove(uri, out hostTransportChannel);
            hostTransportChannel.ChannelCreated -= ChannelCreated;
            hostTransportChannel.ChannelDisconnected -= ChannelDisconnected;
        }

        /// <summary>
        ///     申请基于TCP协议的宿主通道
        /// </summary>
        /// <param name="channel">宿主通道</param>
        /// <returns>返回申请的结果</returns>
        private bool TcpAlloc(TcpHostTransportChannel channel)
        {
            if (_tcpHostChannels.ContainsKey(channel.Port)) return true;
            bool result = _tcpHostChannels.TryAdd(channel.Port, channel);
            if (result)
            {
                channel.ChannelCreated += ChannelCreated;
                channel.ChannelDisconnected += ChannelDisconnected;
            }
            return result;
        }

        /// <summary>
        ///     申请基于TCP协议的宿主通道
        /// </summary>
        /// <param name="channel">宿主通道</param>
        /// <returns>返回申请的结果</returns>
        private bool PipeAlloc(PipeHostTransportChannel channel)
        {
            if (_pipeHostChannels.ContainsKey(channel.LogicalAddress.ToString())) return true;
            bool result = _pipeHostChannels.TryAdd(channel.LogicalAddress.ToString(), channel);
            if (result)
            {
                channel.ChannelCreated += ChannelCreated;
                channel.ChannelDisconnected += ChannelDisconnected;
            }
            return result;
        }

        #endregion

        #region Events

        void ChannelCreated(object sender, LightSingleArgEventArgs<ITransportChannel> e)
        {
            IMessageTransportChannel<Message> msgChannel = new MessageTransportChannel<Message>((IRawTransportChannel)e.Target, ServiceModel.ProtocolStack);
            msgChannel.Disconnected += MessageChannelDisconnected;
            msgChannel.ReceivedMessage += ChannelReceivedMessage;
            _transportChannels.TryAdd(e.Target.Key, msgChannel);
        }

        void MessageChannelDisconnected(object sender, System.EventArgs e)
        {
            IMessageTransportChannel<Message> msgChannel = (IMessageTransportChannel<Message>)sender;
            msgChannel.Close();
            msgChannel.Disconnected -= MessageChannelDisconnected;
            msgChannel.ReceivedMessage -= ChannelReceivedMessage;
            IMessageTransportChannel<Message> removedChannel;
            _transportChannels.TryRemove(msgChannel.Key, out removedChannel);
        }

        void ChannelDisconnected(object sender, LightSingleArgEventArgs<ITransportChannel> e)
        {
            IMessageTransportChannel<Message> removedChannel;
            _transportChannels.TryRemove(e.Target.Key, out removedChannel);
        }

        //传输通道接收到请求消息事件，在NetworkLayer接收到的消息只能是请求消息
        void ChannelReceivedMessage(object sender, LightSingleArgEventArgs<List<Message>> e)
        {
            ServiceModelPerformanceCounter.Instance.RateOfRequest.Increment();
            IMessageTransportChannel<Message> channel = (IMessageTransportChannel<Message>) sender;
            foreach (Message message in e.Target)
            {
                RequestServiceMessage requestServiceMessage = (RequestServiceMessage)message;
                ServiceHandle serviceHandle = ServiceCenter.GetHandle(requestServiceMessage.LogicalRequestAddress);
                if (serviceHandle == null)
                {
                    Logs.Logger.Log("Cannot dispatch a request message to ServiceHandle, Invaild logical address. #address: " + requestServiceMessage.LogicalRequestAddress);
                    continue;
                }
                //派发
                RPCTransaction rpcTrans = new RPCTransaction(requestServiceMessage.TransactionIdentity, channel);
                rpcTrans.Request = requestServiceMessage;
                _tracing.Info("L: {0}\r\nR: {1}\r\n{2}", channel.LocalEndPoint, channel.RemoteEndPoint, requestServiceMessage.ToString());
                serviceHandle.PostRequest(rpcTrans);
            }
        }

        #endregion
    }
}