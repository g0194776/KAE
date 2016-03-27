using Newtonsoft.Json;

namespace KJFramework.ApplicationEngine.Objects
{
    /// <summary>
    ///    远程KIS所使用的内部数据结构
    /// </summary>
    public sealed class PackageInfo
    {
        #region Members.

        /// <summary>
        ///    获取或设置KPP友好名称
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        /// <summary>
        ///    获取或设置KPP完整的包名
        /// </summary>
        [JsonProperty(PropertyName = "pkgname")]
        public string PackageName { get; set; }
        /// <summary>
        ///    获取或设置KPP描述信息
        /// </summary>
        [JsonProperty(PropertyName = "desc")]
        public string Description { get; set; }
        /// <summary>
        ///    获取或设置KPP唯一标示
        /// </summary>
        [JsonProperty(PropertyName = "identity")]
        public string Identity { get; set; }
        /// <summary>
        ///    获取或设置KPP远程可访问的URL地址
        /// </summary>
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
        /// <summary>
        ///    获取或设置KPP等级
        /// </summary>
        [JsonProperty(PropertyName = "level")]
        public string Level { get; set; }
        /// <summary>
        ///    获取或设置KPP计算后的CRC
        /// </summary>
        [JsonProperty(PropertyName = "crc")]
        public ulong CRC { get; set; }

        #endregion
    }
}