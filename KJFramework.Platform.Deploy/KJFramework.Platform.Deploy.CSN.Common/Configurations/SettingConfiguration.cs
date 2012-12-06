using KJFramework.Attribute;

namespace KJFramework.Platform.Deploy.CSN.Common.Configurations
{
    /// <summary>
    ///   KJFramework.ServiceModel配置项
    /// </summary>
    public sealed class SettingConfiguration
    {
        /// <summary>
        ///   CSN开放的端口号
        /// </summary>
        [CustomerField("HostPort")]
        public int HostPort;
        /// <summary>
        ///     服务中心的地址
        /// </summary>
        [CustomerField("CenterAddress")]
        public string CenterAddress;
        /// <summary>
        ///     服务中心的端口
        /// </summary>
        [CustomerField("CenterPort")]
        public int CenterPort;
        /// <summary>
        ///     注册超时时间
        /// </summary>
        [CustomerField("RegistTimeout")]
        public int RegistTimeout;
        /// <summary>
        ///     心跳间隔
        /// </summary>
        [CustomerField("HeartBeatInterval")]
        public int HeartBeatInterval;
        /// <summary>
        ///     重连时间间隔
        /// </summary>
        [CustomerField("ReconnectTimeout")]
        public int ReconnectTimeout;
        /// <summary>
        ///     缓存对象超时检查间隔
        /// </summary>
        [CustomerField("CacheTimeoutCheckInterval")]
        public int CacheTimeoutCheckInterval;
        /// <summary>
        ///     缓存对象存活时间
        /// </summary>
        [CustomerField("CacheLiveTime")]
        public string CacheLiveTime;
        /// <summary>
        ///     最大数据段大小
        ///     <para>* 此字段用于分包数据的大小的判断</para>
        /// </summary>
        [CustomerField("MaxDataChunkSize")]
        public int MaxDataChunkSize;
    }
}