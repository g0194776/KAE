using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.ApplicationEngine.Managers;
using KJFramework.ApplicationEngine.Objects;
using KJFramework.EventArgs;
using KJFramework.Net.Identities;
using KJFramework.Net.Transaction.Objects;
using KJFramework.Net.Uri;
using KJFramework.Tracing;
using ZooKeeperNet;
using Uri = KJFramework.Net.Uri.Uri;

namespace KJFramework.ApplicationEngine.Proxies
{
    /// <summary>
    ///     远程协议注册器
    /// </summary>
    internal sealed class ZooKeeperProtocolRegister : IRemotingProtocolRegister
    {
        #region Constructor.

        /// <summary>
        ///     远程协议注册器
        /// </summary>
        /// <param name="connectionStr">远程ZooKeeper服务器的连接字符串</param>
        /// <param name="sessionTimeout">ZooKeeper的会话超时时间</param>
        public ZooKeeperProtocolRegister(string connectionStr, TimeSpan sessionTimeout)
        {
            if (string.IsNullOrEmpty(connectionStr)) throw new ArgumentNullException("connectionStr");
            _client = new ZooKeeper(connectionStr, sessionTimeout, null);
        }

        #endregion

        #region Members.

        private string _basePath;
        private string _protocolPath;
        private readonly ZooKeeper _client;
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof (ZooKeeperProtocolRegister));

        #endregion

        #region Methods.

        /// <summary>
        ///     初始化
        /// </summary>
        public void Initialize(string hostName, TcpUri basicCommunicationAddress)
        {
            _basePath = SystemWorker.ConfigurationProxy.GetField("KAEWorker", "ZooKeeperBasePath");
            if (_client.Exists(_basePath, false) == null)
            {
                try { _client.Create(_basePath, null, Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent); }
                catch (System.Exception) { }
            }
            //hosting node.
            string hostingPath = Path.Combine(_basePath, "hosting");
            hostingPath = hostingPath.Replace("\\", "/");
            if (_client.Exists(hostingPath, false) == null)
            {
                try { _client.Create(hostingPath, null, Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent); }
                catch (System.Exception) { }
            }
            //protocols node.
           _protocolPath = Path.Combine(_basePath, "protocols");
           _protocolPath = _protocolPath.Replace("\\", "/");
           if (_client.Exists(_protocolPath, false) == null)
            {
                try { _client.Create(_protocolPath, null, Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent); }
                catch (System.Exception) { }
            }
            //create resource node.
            string resourcePath = Path.Combine(_basePath, "hosting/", hostName);
            resourcePath = resourcePath.Replace("\\", "/");
            if (_client.Exists(resourcePath, false) == null)
            {
                try { _client.Create(resourcePath, Encoding.UTF8.GetBytes(basicCommunicationAddress.ToString()), Ids.OPEN_ACL_UNSAFE, CreateMode.Ephemeral); }
                catch (System.Exception) { }
            }
        }

        /// <summary>
        ///     从远程服务器中上架指定的KPP实例并添加所有相关的网络协议
        /// </summary>
        /// <param name="app">待下架的KPP实例</param>
        public void Register(ApplicationDynamicObject app)
        {
            string uniqueId = app.GlobalUniqueId.ToString();
            IDictionary<ProtocolTypes, IList<MessageIdentity>> protocols = app.AcquireSupportedProtocols();
            if (protocols == null || protocols.Count == 0) return;
            foreach (KeyValuePair<ProtocolTypes, IList<MessageIdentity>> pair in protocols)
            {
                if (pair.Value == null || pair.Value.Count == 0) continue;
                Uri networkUri = KAEHostNetworkResourceManager.GetResourceUri(pair.Key);
                foreach (MessageIdentity identity in pair.Value)
                {
                    try
                    {
                        string path = string.Format("{0}/{1}-{2}-{3}-{4}-{5}", _protocolPath, identity.ProtocolId, identity.ServiceId, identity.DetailsId, pair.Key, app.Level);
                        if (_client.Exists(path, false) == null) AddPath(path, CreateMode.Persistent);
                        string subPath = string.Format("{0}/{1};{2}", path, networkUri.Address, uniqueId);
                        AddPath(subPath, CreateMode.Ephemeral);
                    }
                    catch (System.Exception ex) { _tracing.Error(ex, null); }
                }
            }
        }

        /// <summary>
        ///     将一个业务的通信协议与远程可访问地址注册到服务器上
        /// </summary>
        /// <param name="identity">业务协议编号</param>
        /// <param name="protocolTypes">通信协议类型</param>
        /// <param name="level">KAE应用等级</param>
        /// <param name="resourceUri">远程可访问的资源地址</param>
        /// <param name="kppUniqueId">KPP全局唯一编号</param>
        public void Register(MessageIdentity identity, ProtocolTypes protocolTypes, ApplicationLevel level, Uri resourceUri, Guid kppUniqueId)
        {
            if(resourceUri == null) throw new ArgumentNullException("resourceUri");
            string path = string.Format("{0}/{1}-{2}-{3}-{4}-{5}", _protocolPath, identity.ProtocolId, identity.ServiceId, identity.DetailsId, protocolTypes, level);
            AddPath(path, CreateMode.Persistent);
            path += string.Format("/{0};{1}", resourceUri.Address, kppUniqueId);
            AddPath(path, CreateMode.Ephemeral);
        }

        /// <summary>
        ///     从远程服务器中下架指定的KPP实例并抹除所有相关的网络协议
        /// </summary>
        /// <param name="app">待下架的KPP实例</param>
        public void UnRegister(ApplicationDynamicObject app)
        {
            string uniqueId = app.GlobalUniqueId.ToString();
            IDictionary<ProtocolTypes, IList<MessageIdentity>> protocols = app.AcquireSupportedProtocols();
            if (protocols == null || protocols.Count == 0) return;
            foreach (KeyValuePair<ProtocolTypes, IList<MessageIdentity>> pair in protocols)
            {
                if(pair.Value == null || pair.Value.Count == 0) continue;
                foreach (MessageIdentity identity in pair.Value)
                {
                    try
                    {
                        string path = string.Format("{0}/{1}-{2}-{3}-{4}-{5}", _protocolPath, identity.ProtocolId, identity.ServiceId, identity.DetailsId, pair.Key, app.Level);
                        if (_client.Exists(path, false) == null) continue;
                        IList<string> children = _client.GetChildren(path, false).ToList();
                        if (children.Count == 0) continue;
                        foreach (string subPath in children)
                        {
                            if (!subPath.Contains(uniqueId)) continue;
                            try { _client.Delete(subPath, 0); }
                            catch (System.Exception ex) { _tracing.Error(ex, null); }
                        }
                    }
                    catch (System.Exception ex) { _tracing.Error(ex, null); }
                }
            }
        }

        /// <summary>
        ///    根据一组参数获取相应的远程目标资源
        /// </summary>
        /// <param name="level">应用等级</param>
        /// <param name="protocol">通信协议</param>
        /// <param name="protocolTypes">协议类型</param>
        /// <returns>返回远程目标可访问资源</returns>
        public IProtocolResource GetProtocolResource(Protocols protocol, ProtocolTypes protocolTypes, ApplicationLevel level)
        {
            string path = string.Format("{0}/{1}-{2}-{3}-{4}-{5}", _protocolPath, protocol.ProtocolId, protocol.ServiceId, protocol.DetailsId, protocolTypes, level);
            IProtocolResource resource = new ProtocolResource(_client, path, protocol, protocolTypes, level);
            resource.ChildrenChanged += delegate(object sender, System.EventArgs args) { OnChildrenChanged(new LightSingleArgEventArgs<IProtocolResource>((IProtocolResource) sender));};
            return resource;
        }

        private void AddPath(string path, CreateMode mode)
        {
            if (_client.Exists(path, false) != null) return;
            try { _client.Create(path, null, Ids.OPEN_ACL_UNSAFE, mode); }
            catch (KeeperException.NodeExistsException) { }
        }

        #endregion

        #region Events.

        /// <summary>
        ///    远程资源列表变更事件
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<IProtocolResource>> ChildrenChanged;

        private void OnChildrenChanged(LightSingleArgEventArgs<IProtocolResource> e)
        {
            EventHandler<LightSingleArgEventArgs<IProtocolResource>> handler = ChildrenChanged;
            if (handler != null) handler(this, e);
        }

        #endregion
    }
}