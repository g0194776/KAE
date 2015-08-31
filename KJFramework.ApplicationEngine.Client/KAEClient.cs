using System;
using KJFramework.ApplicationEngine.Client.Proxies;
using KJFramework.ApplicationEngine.Proxies;

namespace KJFramework.ApplicationEngine.Client
{
    /// <summary>
    ///      KAE客户端
    /// </summary>
    public static class KAEClient
    {
        #region Members.

        private static string _roleName;
        private static IRemotingProtocolRegister _protocolRegister;
        private static InternalIRemoteConfigurationProxy _configurationProxy;

        /// <summary>
        ///     获取当前是否已经初始化成功
        /// </summary>
        public static bool IsInitialized { get; private set; }

        #endregion

        #region Methods.

        public static void Initialize(string roleName, string zookeeperAddresses, TimeSpan sessionTimeout)
        {
            if (IsInitialized) return;
            if (string.IsNullOrEmpty(roleName)) throw new ArgumentNullException("roleName");
            _roleName = roleName;
            _protocolRegister = new ZooKeeperProtocolRegister(zookeeperAddresses, sessionTimeout);
            _configurationProxy = new InternalIRemoteConfigurationProxy();
        }

        #endregion

        #region Events.


        #endregion
    }
}