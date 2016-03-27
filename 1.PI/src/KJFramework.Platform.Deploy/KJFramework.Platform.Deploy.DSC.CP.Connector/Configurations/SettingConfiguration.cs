using KJFramework.Attribute;

namespace KJFramework.Platform.Deploy.DSC.CP.Connector.Configurations
{
    /// <summary>
    ///   KJFramework.ServiceModel配置项
    /// </summary>
    public sealed class SettingConfiguration
    {
        /// <summary>
        ///   DSC开放的端口号
        /// </summary>
        [CustomerField("HostPort")]
        public int HostPort;
    }
}