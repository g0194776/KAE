using System;
using System.Collections.Generic;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.ApplicationEngine.Proxies;
using KJFramework.Net.Transaction.Objects;

namespace KJFramework.ApplicationEngine.Client.Proxies
{
    /// <summary>
    ///     KAE客户端资源代理器
    /// </summary>
    internal class KAEClientResourceProxy : MarshalByRefObject, IKAEResourceProxy
    {
        #region Constructor.

        /// <summary>
        ///     KAE客户端资源代理器
        /// </summary>
        /// <param name="protocolRegister">远程协议注册器</param>
        /// <param name="configurationProxy">远程配置信息代理器</param>
        public KAEClientResourceProxy(IRemotingProtocolRegister protocolRegister, InternalIRemoteConfigurationProxy configurationProxy)
        {
            _protocolRegister = protocolRegister;
            _configurationProxy = configurationProxy;
        }

        #endregion

        #region Members.

        private readonly IRemotingProtocolRegister _protocolRegister;
        private readonly InternalIRemoteConfigurationProxy _configurationProxy;

        #endregion

        #region Methods.

        /// <summary>
        ///     根据一个角色名和一个配置项的KEY名称来获取一个配置信息
        /// </summary>
        /// <param name="role">角色名称</param>
        /// <param name="field">配置信息的KEY</param>
        /// <returns>返回相应的配置信息</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public string GetField(string role, string field)
        {
            if (string.IsNullOrEmpty(role)) throw new ArgumentNullException("role");
            if (string.IsNullOrEmpty(field)) throw new ArgumentNullException("field");
            return _configurationProxy.GetField(role, field);
        }

        /// <summary>
        ///     根据一组参数向KAE宿主获取远程目标地址的集合
        /// </summary>
        /// <param name="appUniqueId">索取远程目标地址的源KAE APP唯一ID</param>
        /// <param name="protocol">业务协议编号</param>
        /// <param name="level">KAE应用等级</param>
        /// <param name="protocolTypes">通信协议类型</param>
        /// <returns>返回远程目标可访问地址的集合, 如果返回null, 则证明不存在指定条件的远程目标</returns>
        public IList<string> GetRemoteAddresses(Guid appUniqueId, Protocols protocol, ProtocolTypes protocolTypes, ApplicationLevel level)
        {
            IProtocolResource protocolResource = _protocolRegister.GetProtocolResource(protocol, protocolTypes, level);
            protocolResource.RegisterInterestedApp(appUniqueId);
            IList<string> result = protocolResource.GetResult();
            return result;
        }

        #endregion
    }
}