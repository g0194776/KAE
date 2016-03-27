using KJFramework.Attribute;

namespace KJFramework.Platform.Deploy.DSC.CP.Client.Configurations
{
    /// <summary>
    ///   KJFramework.ServiceModel配置项
    /// </summary>
    public sealed class ClientSettingConfiguration
    {
        /// <summary>
        ///   Client Proxy开放的端口号
        /// </summary>
        [CustomerField("ProxyPort")]
        public int ProxyPort;
    }
}