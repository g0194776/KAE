using KJFramework.Attribute;

namespace KJFramework.Data.Synchronization.Configurations
{
    /// <summary>
    ///   Addresses配置项
    /// </summary>
    public sealed class SyncDataSettingConfiguration
    {
        /// <summary>
        ///    事务超时时间
        /// </summary>
        [CustomerField("TranTimeout")]
        public string TranTimeout;
        /// <summary>
        ///    事务超时检查周期时间
        /// </summary>
        [CustomerField("TranChkInterval")] 
        public string TranChkInterval;
    }
}