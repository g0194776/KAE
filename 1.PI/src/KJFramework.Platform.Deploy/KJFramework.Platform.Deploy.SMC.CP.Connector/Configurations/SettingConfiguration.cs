using KJFramework.Attribute;

namespace KJFramework.Platform.Deploy.SMC.CP.Connector.Configurations
{
    /// <summary>
    ///   KJFramework.ServiceModel配置项
    /// </summary>
    public sealed class SettingConfiguration
    {
        /// <summary>
        ///   服务中心的地址
        /// </summary>
        [CustomerField("CenterAddress")]
        public string CenterAddress;
        /// <summary>
        ///   服务中心的端口
        /// </summary>
        [CustomerField("CenterPort")]
        public int CenterPort;
        /// <summary>
        ///   服务中心注册超时时间
        /// </summary>
        [CustomerField("RegistTimeout")]
        public int RegistTimeout;
        /// <summary>
        ///   服务中心重连时间
        /// </summary>
        [CustomerField("ReconnectTimeout")]
        public int ReconnectTimeout;
        /// <summary>
        ///   SMC开放的端口号
        /// </summary>
        [CustomerField("HostPort")]
        public int HostPort;
        /// <summary>
        ///   与服务中心的心跳时间间隔
        ///    <para>* 单位：毫秒</para>
        /// </summary>
        [CustomerField("HeartBeatInterval")]
        public int HeartBeatInterval;
        /// <summary>
        ///   与动态服务的心跳时间间隔
        ///    <para>* 单位：毫秒</para>
        /// </summary>
        [CustomerField("CheckInterval")]
        public int CheckInterval;
    }
}