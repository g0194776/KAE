using KJFramework.Attribute;

namespace KJFramework.Data.Synchronization.Configurations
{
    /// <summary>
    ///   Addresses������
    /// </summary>
    public sealed class SyncDataSettingConfiguration
    {
        /// <summary>
        ///    ����ʱʱ��
        /// </summary>
        [CustomerField("TranTimeout")]
        public string TranTimeout;
        /// <summary>
        ///    ����ʱ�������ʱ��
        /// </summary>
        [CustomerField("TranChkInterval")] 
        public string TranChkInterval;
    }
}