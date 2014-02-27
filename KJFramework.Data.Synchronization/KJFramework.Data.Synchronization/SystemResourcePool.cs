using KJFramework.Data.Synchronization.Enums;
using KJFramework.Messages.Contracts;
using KJFramework.Net.Channels;
using System;
using System.Collections.Concurrent;
using System.Net;

namespace KJFramework.Data.Synchronization
{
    /// <summary>
    ///     �ڲ�ϵͳ��Դ��
    /// </summary>
    internal class SystemResourcePool : ISystemResourcePool
    {
        #region Members

        private static readonly ConcurrentDictionary<string, IMessageTransportChannel<MetadataContainer>> _channels = new ConcurrentDictionary<string, IMessageTransportChannel<MetadataContainer>>();
        private static readonly ConcurrentDictionary<string, PublisherResourceStub> _hosts = new ConcurrentDictionary<string, PublisherResourceStub>();
        /// <summary>
        ///     �ڲ�ϵͳ��Դ��
        /// </summary>
        public static readonly SystemResourcePool Instance = new SystemResourcePool();

        #endregion

        #region Implementation of ISystemResourcePool

        /// <summary>
        ///     ���һ��������Դ
        /// </summary>
        /// <param name="res">������Դ</param>
        /// <returns>������Ӻ��״̬</returns>
        /// <exception cref="System.NullReferenceException">��Ч����</exception>
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
        ///     ͨ��һ��������Դ��ȡһ��ͨ���ŵ�
        ///     <para>* �������ָ���������ŵ������ڣ����ᴴ����ͨ���ŵ�</para>
        /// </summary>
        /// <param name="res">������Դ</param>
        /// <returns>����ͨ���ŵ�</returns>
        /// <exception cref="System.NullReferenceException">��Ч����</exception>
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