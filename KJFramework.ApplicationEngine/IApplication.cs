﻿using System;
using System.Collections.Generic;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.ApplicationEngine.Resources;
using KJFramework.Dynamic.Components;

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

        #endregion

        #region Methods.

        /// <summary>
        ///    获取应用内部所有支持的网络资源列表
        /// </summary>
        /// <returns>返回支持的网络资源列表</returns>
        IList<KAENetworkResource> AcquireCommunicationSupport();

        #endregion
    }
}