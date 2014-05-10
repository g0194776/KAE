using KJFramework.Attribute;

namespace KJFramework.ApplicationEngine.Configurations
{
    /// <summary>
    ///   Addresses配置项
    /// </summary>
    public sealed class DeploySettingConfiguration
    {
        /// <summary>
        ///   CSN服务地址
        /// </summary>
        [CustomerField("CSN")]
        public string CSN;
    }
}