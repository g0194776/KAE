using Newtonsoft.Json;

namespace KJFramework.ApplicationEngine.Objects
{
    /// <summary>
    ///    装配清单用到的内部数据结构
    /// </summary>
    internal sealed class Package
    {
        #region Members.

        /// <summary>
        ///    获取或设置远程KPP包的访问名称
        /// </summary>
        [JsonProperty(PropertyName = "package")]
        public string Name { get; set; }

        #endregion
    }
}