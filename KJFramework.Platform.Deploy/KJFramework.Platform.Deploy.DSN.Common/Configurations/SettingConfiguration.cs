using KJFramework.Attribute;

namespace KJFramework.Platform.Deploy.DSN.Common.Configurations
{
    /// <summary>
    ///   KJFramework.ServiceModel������
    /// </summary>
    public sealed class SettingConfiguration
    {
        /// <summary>
        ///   DSN���ŵĶ˿ں�
        /// </summary>
        [CustomerField("HostPort")]
        public int HostPort;
        /// <summary>
        ///     �������ĵĵ�ַ
        /// </summary>
        [CustomerField("CenterAddress")]
        public string CenterAddress;
        /// <summary>
        ///     �������ĵĶ˿�
        /// </summary>
        [CustomerField("CenterPort")]
        public int CenterPort;
        /// <summary>
        ///     ע�ᳬʱʱ��
        /// </summary>
        [CustomerField("RegistTimeout")]
        public int RegistTimeout;
        /// <summary>
        ///     �������
        /// </summary>
        [CustomerField("HeartBeatInterval")]
        public int HeartBeatInterval;
        /// <summary>
        ///     ����ʱ����
        /// </summary>
        [CustomerField("ReconnectTimeout")]
        public int ReconnectTimeout;
        /// <summary>
        ///     ����Ŀ¼
        /// </summary>
        [CustomerField("ManageDir")]
        public string ManageDir;
        /// <summary>
        ///     ѹ��������Ŀ¼
        /// </summary>
        [CustomerField("SaveDir")]
        public string SaveDir;
    }
}