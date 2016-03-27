using System.Collections.Generic;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.Net.Identities;
using KJFramework.Net.Transaction.Objects;

namespace KJFramework.ApplicationEngine.Entities
{
    /// <summary>
    ///     应用信息集合
    /// </summary>
    public sealed class ApplicationInformation
    {
        #region Members.

        /// <summary>
        ///     获取或设置包名
        /// </summary>
        public string PackageName { get; internal set; }
        /// <summary>
        ///     获取或设置版本
        /// </summary>
        public string Version { get; internal set; }
        /// <summary>
        ///     获取或设置描述
        /// </summary>
        public string Description { get; internal set; }
        /// <summary>
        ///     获取或设置应用等级
        /// </summary>
        public ApplicationLevel Level { get; internal set; }
        /// <summary>
        ///     获取或设置应用所支持的消息协议信息集合
        /// </summary>
        public IDictionary<ProtocolTypes, IList<MessageIdentity>> MessageIdentities { get; internal set; }

        #endregion
    }
}