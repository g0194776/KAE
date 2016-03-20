﻿using System.Collections.Generic;
using KJFramework.ApplicationEngine.Eums;

namespace KJFramework.ApplicationEngine
{
    /// <summary>
    ///    全局KAE内部设置
    /// </summary>
    internal static class KAESettings
    {
        #region Members.

        /// <summary>
        ///    KAE内部支持TDD测试的全局变量
        /// </summary>
        public static bool IsTDDTesting = false;
        /// <summary>
        ///    KAE内部支持的协议
        /// </summary>
        public static readonly IList<ProtocolTypes> SupportedProtocols = new[] { ProtocolTypes.Metadata, ProtocolTypes.INTERNAL_SPECIAL_RESOURCE };
        /// <summary>
        ///    KAE内部支持的协议数量
        /// </summary>
        public static int SupportedProtocolCount = SupportedProtocols.Count;

        #endregion
    }
}