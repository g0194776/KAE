using System;
using System.Collections.Generic;
using System.Net;
using KJFramework.ApplicationEngine;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.ApplicationEngine.Proxies;
using KJFramework.Net.Transaction.Objects;

namespace KAELoadTesting
{
    public class LoadingTestProxy : IKAEResourceProxy
    {
        private readonly IPEndPoint _iep;

        #region Constructor.

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public LoadingTestProxy(IPEndPoint iep)
        {
            _iep = iep;
        }

        #endregion

        #region Implementation of IKAEResourceProxy

        /// <summary>
        ///     根据一个角色名和一个配置项的KEY名称来获取一个配置信息
        /// </summary>
        /// <param name="role">角色名称</param>
        /// <param name="field">配置信息的KEY</param>
        /// <returns>返回相应的配置信息</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public string GetField(string role, string field)
        {
            return SystemWorker.ConfigurationProxy.GetField(role, field, false);
        }

        /// <summary>
        ///     根据一组参数向KAE宿主获取远程目标地址的集合
        /// </summary>
        /// <param name="appName">目标APP的名称</param>
        /// <param name="version">目标APP的版本</param>
        /// <param name="appUniqueId">索取远程目标地址的源KAE APP唯一ID</param>
        /// <param name="protocol">业务协议编号</param>
        /// <param name="level">KAE应用等级</param>
        /// <param name="protocolTypes">通信协议类型</param>
        /// <returns>返回远程目标可访问地址的集合, 如果返回null, 则证明不存在指定条件的远程目标</returns>
        public IList<string> GetRemoteAddresses(string appName, string version, Guid appUniqueId, Protocols protocol,
            ProtocolTypes protocolTypes, ApplicationLevel level)
        {
            return new List<string> { _iep.ToString() };
        }

        #endregion
    }
}