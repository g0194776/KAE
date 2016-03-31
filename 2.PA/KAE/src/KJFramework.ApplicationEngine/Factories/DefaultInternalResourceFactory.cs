﻿using System.Collections.Generic;
using KJFramework.ApplicationEngine.Finders;
using KJFramework.ApplicationEngine.Proxies;

namespace KJFramework.ApplicationEngine.Factories
{
    /// <summary>
    ///    KAE内部所使用的默认资源工厂
    /// </summary>
    internal class DefaultInternalResourceFactory : IInternalResourceFactory
    {
        #region Members.

        private IDictionary<string, object> _resources = new Dictionary<string, object>();
        
        #endregion

        #region Methods.

        /// <summary>
        ///    初始化当前资源工厂
        /// </summary>
        public void Initialize()
        {
            _resources.Add(KAESystemInternalResource.APPDownloader, new RemotingApplicationDownloader());
            _resources.Add(KAESystemInternalResource.APPFinder, new DefaultApplicationFinder());
            _resources.Add(KAESystemInternalResource.KISProxy, new RemotingKISProxy(SystemWorker.ConfigurationProxy.GetField("kae-system", "KIS-Address", false)));
            //ZooKeeperProtocolRegister protocolRegister = new ZooKeeperProtocolRegister(SystemWorker.ConfigurationProxy.GetField("kae-system", "ZooKeeper-Addresses", false), TimeSpan.Parse(SystemWorker.ConfigurationProxy.GetField("kae-system", "ZooKeeper-SessionTimeout", false)));
            //protocolRegister.Initialize(Environment.GetEnvironmentVariable("COMPUTERNAME"), (TcpUri) KAEHostNetworkResourceManager.GetResourceUri(ProtocolTypes.INTERNAL_SPECIAL_RESOURCE));
            //_resources.Add(KAESystemInternalResource.ProtocolRegister, protocolRegister);
        }

        /// <summary>
        ///    通过一个字符串的全名称来获取指定的资源
        /// </summary>
        /// <param name="fullname">资源的全名称</param>
        /// <returns>返回资源实例，如果不存在则返回null</returns>
        public object GetResource(string fullname)
        {
            object obj;
            return (_resources.TryGetValue(fullname, out obj) ? obj : null);
        }

        #endregion
    }
}