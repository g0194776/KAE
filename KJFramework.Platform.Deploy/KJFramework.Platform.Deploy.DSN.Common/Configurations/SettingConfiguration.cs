using KJFramework.Attribute;

namespace KJFramework.Platform.Deploy.DSN.Common.Configurations
{
    /// <summary>
    ///   KJFramework.ServiceModel配置项
    /// </summary>
    public sealed class SettingConfiguration
    {
        /// <summary>
        ///   DSN开放的端口号
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
        ///     管理目录
        /// </summary>
        [CustomerField("ManageDir")]
        public string ManageDir;
        /// <summary>
        ///     压缩包保存目录
        /// </summary>
        [CustomerField("SaveDir")]
        public string SaveDir;
    }
}