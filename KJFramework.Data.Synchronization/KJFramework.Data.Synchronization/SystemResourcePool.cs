using KJFramework.Data.Synchronization.Enums;
using KJFramework.Messages.Contracts;
using KJFramework.Net.Channels;
using System;
using System.Collections.Concurrent;
using System.Net;

namespace KJFramework.Data.Synchronization
{
    /// <summary>
    ///     内部系统资源池
    /// </summary>
    internal class SystemResourcePool : ISystemResourcePool
    {
        #region Members

        private static readonly ConcurrentDictionary<string, IMessageTransportChannel<MetadataContainer>> _channels = new ConcurrentDictionary<string, IMessageTransportChannel<MetadataContainer>>();
        private static readonly ConcurrentDictionary<string, PublisherResourceStub> _hosts = new ConcurrentDictionary<string, PublisherResourceStub>();
        /// <summary>
        ///     内部系统资源池
        /// </summary>
        public static readonly SystemResourcePool Instance = new SystemResourcePool();

        #endregion

        #region Implementation of ISystemResourcePool

        /// <summary>
        ///     添加一个网络资源
        /// </summary>
        /// <param name="res">网络资源</param>
        /// <returns>返回添加后的状态</returns>
        /// <exception cref="System.NullReferenceException">无效参数</exception>
        public PublisherResourceStub Regist(INetworkResource res)
        {
            if (res == null) throw new ArgumentNullException("res");
            if (res.Mode != ResourceMode.Local) throw new ArgumentException("Cannot support this mode at LOCAL resource. #mode: " + res.Mode);
            PublisherResourceStub stub;
            if (_hosts.TryGetValue(res.ToString(), out stub))
            {
                stub.AddUseRef();
                return stub;
            }
            PublisherResourceStub resourceStub = new PublisherResourceStub();
            resourceStub.Bind(res);
            resourceStub.AddUseRef();
            if (!_hosts.TryAdd(res.ToString(), resourceStub)) throw new System.Exception("Cannot add binded LOCAL resource stub!");
            resourceStub.Disposed += StubDisposed;
            return resourceStub;
        }

        /// <summary>
        ///     通过一个网络资源获取一个通信信道
        ///     <para>* 如果具有指定条件的信道不存在，将会创建该通信信道</para>
        /// </summary>
        /// <param name="res">网络资源</param>
        /// <returns>返回通信信道</returns>
        /// <exception cref="System.NullReferenceException">无效参数</exception>
        public IMessageTransportChannel<MetadataContainer> GetChannel(INetworkResource res)
        {
            if (res == null) throw new ArgumentNullException("res");
            if (res.Mode != ResourceMode.Remote) throw new ArgumentException("Cannot support this mode at REMOTE resource. #mode: " + res.Mode);
            IMessageTransportChannel<MetadataContainer> channel;
            if (_channels.TryGetValue(res.ToString(), out channel)) return channel;
            TcpTransportChannel tcpChannel = new TcpTransportChannel(res.GetResource<IPEndPoint>());
            tcpChannel.Connect();
            if (tcpChannel.IsConnected) channel = new MessageTransportChannel<MetadataContainer>(tcpChannel, Global.ProtocolStack);
            return tcpChannel.IsConnected ? (_channels.TryAdd(res.ToString(), channel) ? channel : null) : null;
        }

        #endregion

        #region Events

        void StubDisposed(object sender, System.EventArgs e)
        {
            PublisherResourceStub s;
            PublisherResourceStub stub = (PublisherResourceStub)sender;
            stub.Disposed -= StubDisposed;
            _hosts.TryRemove(stub.ResourceKey, out s);
        }

        #endregion
    }
}