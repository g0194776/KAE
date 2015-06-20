using System;
using System.Collections.Generic;
using System.Linq;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.Net.Transaction.Objects;
using ZooKeeperNet;

namespace KJFramework.ApplicationEngine.Proxies
{
    /// <summary>
    ///     Զ��Ŀ��ɷ�����Դ
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
        ///     Զ��Ŀ��ɷ�����Դ
        /// </summary>
        /// <param name="client">ZooKeeper�ͻ���ʵ��</param>
        /// <param name="path">����Ҫ��ע��Զ��ZooKeeper·��</param>
        /// <param name="level">Ӧ�õȼ�</param>
        /// <param name="protocol">ͨ��Э��</param>
        /// <param name="protocolTypes">Э������</param>
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
        ///    ��ȡ����Ҫ��ע��Զ��ZooKeeper·��
        /// </summary>
        public string Path
        {
            get { return _path; }
        }

        /// <summary>
        ///    ��ȡ����ע��ҵ��ͨ��Э��
        /// </summary>
        public Protocols Protocol
        {
            get { return _protocol; }
        }

        /// <summary>
        ///    ��ȡӦ�õȼ�
        /// </summary>
        public ApplicationLevel Level
        {
            get { return _level; }
        }

        /// <summary>
        ///    ��ȡ��ʹ�õ�Э������
        /// </summary>
        public ProtocolTypes ProtocolTypes
        {
            get { return _protocolTypes; }
        }

        #endregion

        #region Methods.

        /// <summary>
        ///     ��ȡ�ڲ��Ľ��
        /// </summary>
        /// <returns>�����ڲ�����������</returns>
        public IList<string> GetResult()
        {
            return _client.GetChildren(Path, new ZooKeeperWatcher(@event => OnChildrenChanged())).ToList();
        }

        /// <summary>
        ///     ����һ���Ե�ǰҵ��Э�����Ȥ��KAE APP��Ϣ������
        /// </summary>
        /// <param name="appUniqueId">KAE APPΨһ���</param>
        public void RegisterInterestedApp(Guid appUniqueId)
        {
            _interestedApps.Add(appUniqueId);
        }

        /// <summary>
        ///    ��ȡһ���Ե�ǰԶ����Դ����Ȥ��KAE APPΨһ����б�
        /// </summary>
        /// <returns>�����ڲ�����������</returns>
        public IEnumerable<Guid> GetInterestedApps()
        {
            return _interestedApps;
        }

        #endregion

        #region Events.

        /// <summary>
        ///    �ڲ���Դ�б����¼�
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