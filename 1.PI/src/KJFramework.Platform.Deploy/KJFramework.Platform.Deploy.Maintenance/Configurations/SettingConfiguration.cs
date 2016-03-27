using KJFramework.Attribute;

namespace KJFramework.Platform.Deploy.Maintenance.Configurations
{
    /// <summary>
    ///   KJFramework.ServiceModel配置项
    /// </summary>
    public sealed class SettingConfiguration
    {
        /// <summary>
        ///   管理地址
        /// </summary>
        [CustomerField("ManageAddress")]
        public string ManageAddress;
        /// <summary>
        ///     管理端口
        /// </summary>
        [CustomerField("ManagePort")]
        public int ManagePort;
        /// <summary>
        ///   与服务中心的心跳时间间隔
        ///    <para>* 单位：毫秒</para>
        /// </summary>
        [CustomerField("HeartBeatInterval")]
        public int HeartBeatInterval;
        /// <summary>
        ///     重新连接的时间间隔
        ///    <para>* 单位：毫秒</para>
        /// </summary>
        [CustomerField("ReconnectInterval")]
        public int ReconnectInterval;
    }
}