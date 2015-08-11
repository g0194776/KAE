using System;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.EventArgs;
using KJFramework.Net.Channels.Identities;
using KJFramework.Net.Transaction.Objects;
using ZooKeeperNet;
using Uri = KJFramework.Net.Channels.Uri.Uri;

namespace KJFramework.ApplicationEngine.Proxies
{
    /// <summary>
    ///     远程协议注册器
    /// </summary>
    internal class ZooKeeperProtocolRegister : IRemotingProtocolRegister
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

        private readonly ZooKeeper _client;

        #endregion

        #region Methods.

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
            string path = string.Format("/{0}-{1}-{2}-{3}-{4}", identity.ProtocolId, identity.ServiceId, identity.DetailsId, protocolTypes, level);
            AddPath(path, CreateMode.Persistent);
            path += string.Format("/{0};{1}", resourceUri.Address, kppUniqueId);
            AddPath(path, CreateMode.Ephemeral);
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
            string path = string.Format("/{0}-{1}-{2}-{3}-{4}", protocol.ProtocolId, protocol.ServiceId, protocol.DetailsId, protocolTypes, level);
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
        protected virtual void OnChildrenChanged(LightSingleArgEventArgs<IProtocolResource> e)
        {
            EventHandler<LightSingleArgEventArgs<IProtocolResource>> handler = ChildrenChanged;
            if (handler != null) handler(this, e);
        }

        #endregion
    }
}