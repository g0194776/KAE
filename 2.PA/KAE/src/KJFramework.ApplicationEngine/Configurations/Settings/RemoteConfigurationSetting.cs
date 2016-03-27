namespace KJFramework.ApplicationEngine.Configurations.Settings
{
    /// <summary>
    ///     远程配置设置
    /// </summary>
    public class RemoteConfigurationSetting
    {
        #region Constructor

        /// <summary>
        ///     远程配置设置
        /// </summary>
        static RemoteConfigurationSetting()
        {
            Default = new RemoteConfigurationSetting();
        }

        #endregion

        #region Members

        /// <summary>
        ///     是否自定义所有KJFramework的配置信息
        /// </summary>
        public bool IsCustomizeKJFrameworkConfig { get; set; }
        /// <summary>
        ///     是否需要自定义KJFramework.Net的配置信息
        /// </summary>
        public bool IsCustomize_KJFramework_Net_Config { get; set; }
        /// <summary>
        ///     是否需要自定义KJFramework.Net.Channels的配置信息
        /// </summary>
        public bool IsCustomize_KJFramework_Net_Channels_Config { get; set; }
        /// <summary>
        ///     是否需要自定义KJFramework.Net.Transaction的配置信息
        /// </summary>
        public bool IsCustomize_KJFramework_Net_Transaction_Config { get; set; }
        /// <summary>
        ///     是否指定部署平台CSN的地址
        /// </summary>
        public bool IsSpecific_Deployment_Address_Config { get; set; }
        /// <summary>
        ///     是否指定使用者信息
        /// </summary>
        public bool IsSpecific_Customer_Profile_Config { get; set; }
        /// <summary>
        ///     是否指定编码集
        /// </summary>
        public bool IsSpecific_System_Element_Config { get; set; }
        /// <summary>
        ///     是否需要更多的.NET FRAMEWORK配置信息
        /// </summary>
        public bool IsCustomize_DotNetFramework_Config { get; set; }
        /// <summary>
        ///     默认配置集
        /// </summary>
        public static RemoteConfigurationSetting Default { get; set; }

        #endregion
    }
}