using System;
using System.Collections.Generic;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.ApplicationEngine.Resources;
using KJFramework.Basic.Enum;
using KJFramework.Dynamic.Components;
using KJFramework.Net.Channels.Identities;

namespace KJFramework.ApplicationEngine
{
    /// <summary>
    ///    KAE应用抽象父类
    /// </summary>
    public abstract class Application : DynamicDomainComponent, IApplication
    {
        #region Members.

        /// <summary>
        ///    获取应用版本
        /// </summary>
        public string Version { get; private set; }
        /// <summary>
        ///    获取应用描述
        /// </summary>
        public string Description { get; private set; }
        /// <summary>
        ///    获取应用包名
        /// </summary>
        public string PackageName { get; private set; }
        /// <summary>
        ///    获取应用的全局唯一编号
        /// </summary>
        public Guid GlobalUniqueId { get; private set; }
        /// <summary>
        ///    获取应用当前的状态
        /// </summary>
        public ApplicationStatus Status { get; private set; }

        #endregion

        #region Methods.

        /// <summary>
        ///    获取应用内部所有支持的网络资源列表
        /// </summary>
        /// <returns>返回支持的网络资源列表</returns>
        public abstract IList<KAENetworkResource> AcquireCommunicationSupport();
        /// <summary>
        ///    获取应用内部所有已经支持的网络通讯协议
        /// </summary>
        /// <returns>返回支持的网络通信协议列表</returns>
        public abstract IDictionary<ProtocolTypes, IList<MessageIdentity>> AcquireSupportedProtocols();
        /// <summary>
        ///    应用初始化
        /// </summary>
        /// <param name="structure">KPP资源包的数据结构</param>
        internal void Initialize(KPPDataStructure structure)
        {
            
        }

        protected override void InnerStart()
        {
        }

        protected override void InnerStop()
        {
        }

        protected override void InnerOnLoading()
        {
        }

        protected override HealthStatus InnerCheckHealth()
        {
            return HealthStatus.Good;
        }

        #endregion
    }
}