using System.Collections.Generic;

using Newtonsoft.Json;

namespace KJFramework.ApplicationEngine.Objects
{
    /// <summary>
    ///    装配清单用到的内部数据结构
    /// </summary>
    internal sealed class PackageList
    {
        #region Members.

        /// <summary>
        ///    获取或设置装配清单中的远程KPP列表
        /// </summary>
        [JsonProperty(PropertyName = "packages")]
        public IList<Package> Packages { get; set; }

        #endregion
    }
}