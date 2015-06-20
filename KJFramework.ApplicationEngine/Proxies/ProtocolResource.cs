using System;
using System.Collections.Generic;
using System.Linq;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.Net.Transaction.Objects;
using ZooKeeperNet;

namespace KJFramework.ApplicationEngine.Proxies
{
    /// <summary>
    ///     远程目标可访问资源
    /// </summary>
    internal class ProtocolResource : IProtocolResource
    {
        #region Members.

        private readonly string _path;
        private readonly ZooKeeper _client;
        private readonly Protocols _protocol;
        private readonly ApplicationLevel _level;
        private readonly IList<Guid> _interestedApps; 
        private readonly ProtocolTypes _protocolTypes;

        #endregion

        #region Constructor.

        /// <summary>
        ///     远程目标可访问资源
        /// </summary>
        /// <param name="client">ZooKeeper客户端实例</param>
        /// <param name="path">所需要关注的远程ZooKeeper路径</param>
        /// <param name="level">应用等级</param>
        /// <param name="protocol">通信协议</param>
        /// <param name="protocolTypes">协议类型</param>
        public ProtocolResource(ZooKeeper client, string path, Protocols protocol, ProtocolTypes protocolTypes, ApplicationLevel level)
        {
            _client = client;
            _path = path;
            _protocol = protocol;
            _protocolTypes = protocolTypes;
            _level = level;
            _interestedApps = new List<Guid>();
        }

        #endregion

        #region Members.

        /// <summary>
        ///    获取所需要关注的远程ZooKeeper路径
        /// </summary>
        public string Path
        {
            get { return _path; }
        }

        /// <summary>
        ///    获取所关注的业务通信协议
        /// </summary>
        public Protocols Protocol
        {
            get { return _protocol; }
        }

        /// <summary>
        ///    获取应用等级
        /// </summary>
        public ApplicationLevel Level
        {
            get { return _level; }
        }

        /// <summary>
        ///    获取所使用的协议类型
        /// </summary>
        public ProtocolTypes ProtocolTypes
        {
            get { return _protocolTypes; }
        }

        #endregion

        #region Methods.

        /// <summary>
        ///     获取内部的结果
        /// </summary>
        /// <returns>返回内部包含的数据</returns>
        public IList<string> GetResult()
        {
            return _client.GetChildren(Path, new ZooKeeperWatcher(@event => OnChildrenChanged())).ToList();
        }

        /// <summary>
        ///     增加一个对当前业务协议感兴趣的KAE APP信息订阅者
        /// </summary>
        /// <param name="appUniqueId">KAE APP唯一编号</param>
        public void RegisterInterestedApp(Guid appUniqueId)
        {
            _interestedApps.Add(appUniqueId);
        }

        /// <summary>
        ///    获取一个对当前远程资源感兴趣的KAE APP唯一编号列表
        /// </summary>
        /// <returns>返回内部包含的数据</returns>
        public IEnumerable<Guid> GetInterestedApps()
        {
            return _interestedApps;
        }

        #endregion

        #region Events.

        /// <summary>
        ///    内部资源列表变更事件
        /// </summary>
        public event EventHandler ChildrenChanged;

        protected virtual void OnChildrenChanged()
        {
            EventHandler handler = ChildrenChanged;
            if (handler != null) handler(this, System.EventArgs.Empty);
        }

        #endregion
    }
}