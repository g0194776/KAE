using System;
using System.Collections.Generic;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.Net.Identities;
using KJFramework.Net.Transaction.Objects;

namespace KJFramework.ApplicationEngine
{
    /// <summary>
    ///    KPP接口
    /// </summary>
    public interface IApplication
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
        /// <summary>
        ///    获取内部所使用的隧道连接地址
        /// </summary>
        string TunnelAddress { get; }

        #endregion

        #region Methods.

        /// <summary>
        ///    更新网络缓存信息
        /// </summary>
        /// <param name="level">应用等级</param>
        /// <param name="cache">远程目标终结点信息列表</param>
        /// <param name="protocol">通信协议</param>
        /// <param name="protocolTypes">协议类型</param>
        void UpdateCache(Protocols protocol, ProtocolTypes protocolTypes, ApplicationLevel level, List<string> cache);

        #endregion
    }
}