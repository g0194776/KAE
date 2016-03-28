﻿using System;
using KJFramework.ApplicationEngine.Factories;

namespace KJFramework.ApplicationEngine
{
    /// <summary>
    ///    KAE内部系统资源
    /// </summary>
    internal static class KAESystemInternalResource
    {
        #region Members.

        /// <summary>
        ///     KAE内部使用的APP下载器
        /// </summary>
        public static string APPDownloader = "KAE.Remoting.ApplicationDownloader";
        /// <summary>
        ///     KAE内部使用的远程信息站访问代理器
        /// </summary>
        public static string KISProxy = "KAE.Remoting.KISProxy";
        /// <summary>
        ///    KAE内部使用的APP查找器
        /// </summary>
        public static string APPFinder = "KAE.Finders.Application";
        /// <summary>
        ///     KAE内部使用的远程协议注册器
        /// </summary>
        [Obsolete("Useless property unless you want to register yours protocol to the remote ZooKeepeer.", true)]
        public static string ProtocolRegister = "KAE.Remoting.ProtocolRegister";
        /// <summary>
        ///     KAE内部使用的资源工厂
        /// </summary>
        public static IInternalResourceFactory Factory;

        #endregion
    }
}