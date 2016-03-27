using KJFramework.Attribute;

namespace KJFramework.Platform.Deploy.DSC.CP.Client.Configurations
{
    /// <summary>
    ///   KJFramework.ServiceModel������
    /// </summary>
    public sealed class ClientSettingConfiguration
    {
        /// <summary>
        ///   Client Proxy���ŵĶ˿ں�
        /// </summary>
        [CustomerField("ProxyPort")]
        public int ProxyPort;
    }
}