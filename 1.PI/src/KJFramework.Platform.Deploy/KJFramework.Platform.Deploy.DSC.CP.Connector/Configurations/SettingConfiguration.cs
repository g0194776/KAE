using KJFramework.Attribute;

namespace KJFramework.Platform.Deploy.DSC.CP.Connector.Configurations
{
    /// <summary>
    ///   KJFramework.ServiceModel������
    /// </summary>
    public sealed class SettingConfiguration
    {
        /// <summary>
        ///   DSC���ŵĶ˿ں�
        /// </summary>
        [CustomerField("HostPort")]
        public int HostPort;
    }
}