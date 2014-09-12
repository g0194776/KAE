using System;
using System.Collections.Generic;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.Dynamic.Components;
using KJFramework.Net.Channels.Identities;

namespace KJFramework.ApplicationEngine
{
    /// <summary>
    ///    KPP接口
    /// </summary>
    public interface IApplication : IDynamicDomainComponent
    {
        #region Members.

        /// <summary>
        ///    获取应用版本
        /// </summary>
        string Version { get; }
        /// <summary>
        ///    获取应用描述
        /// </summary>
        string Description { get; }
        /// <summary>
        ///    获取应用包名
        /// </summary>
        string PackageName { get; }
        /// <summary>
        ///    获取应用的全局唯一编号
        /// </summary>
        Guid GlobalUniqueId { get; }
        /// <summary>
        ///    获取应用当前的状态
        /// </summary>
        ApplicationStatus Status { get; }
        /// <summary>
        ///    获取应用等级
        /// </summary>
        ApplicationLevel Level { get; }
        /// <summary>
        ///    获取一个值，该值标示了当前KPP包裹是否包含了一个完整的运行环境所需要的所有依赖文件
        /// </summary>
        bool IsCompletedEnvironment { get; }

        #endregion

        #region Methods.

        /// <summary>
        ///    获取应用内部所有已经支持的网络通讯协议
        /// </summary>
        /// <returns>返回支持的网络通信协议列表</returns>
        IDictionary<ProtocolTypes, IList<MessageIdentity>> AcquireSupportedProtocols();
        /// <summary>
        ///    更新网络缓存信息
        /// </summary>
        /// <param name="cache">网络信息</param>
        void UpdateNetworkCache(Dictionary<string, List<string>> cache);
        /// <summary>
        ///    更新灰度升级策略的源代码
        /// </summary>
        /// <param name="code">灰度升级策略的源代码</param>
        void UpdateGreyPolicy(string code);

        #endregion
    }
}