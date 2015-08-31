using System;
using KJFramework.ApplicationEngine.Eums;
using Uri = KJFramework.Net.Uri.Uri;

namespace KJFramework.ApplicationEngine.Resources
{
    /// <summary>
    ///    网络资源
    /// </summary>
    public class KAENetworkResource : MarshalByRefObject
    {
        #region Members.

        /// <summary>
        ///    获取或设置协议分类
        /// </summary>
        public ProtocolTypes Protocol { get; set; }
        /// <summary>
        ///    获取或设置网络资源的URI
        /// </summary>
        public Uri NetworkUri { get; set; }

        #endregion
    }
}