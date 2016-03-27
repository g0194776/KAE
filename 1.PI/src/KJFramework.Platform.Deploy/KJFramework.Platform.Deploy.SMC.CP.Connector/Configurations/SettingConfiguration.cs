using KJFramework.Attribute;

namespace KJFramework.Platform.Deploy.SMC.CP.Connector.Configurations
{
    /// <summary>
    ///   KJFramework.ServiceModel������
    /// </summary>
    public sealed class SettingConfiguration
    {
        /// <summary>
        ///   �������ĵĵ�ַ
        /// </summary>
        [CustomerField("CenterAddress")]
        public string CenterAddress;
        /// <summary>
        ///   �������ĵĶ˿�
        /// </summary>
        [CustomerField("CenterPort")]
        public int CenterPort;
        /// <summary>
        ///   ��������ע�ᳬʱʱ��
        /// </summary>
        [CustomerField("RegistTimeout")]
        public int RegistTimeout;
        /// <summary>
        ///   ������������ʱ��
        /// </summary>
        [CustomerField("ReconnectTimeout")]
        public int ReconnectTimeout;
        /// <summary>
        ///   SMC���ŵĶ˿ں�
        /// </summary>
        [CustomerField("HostPort")]
        public int HostPort;
        /// <summary>
        ///   ��������ĵ�����ʱ����
        ///    <para>* ��λ������</para>
        /// </summary>
        [CustomerField("HeartBeatInterval")]
        public int HeartBeatInterval;
        /// <summary>
        ///   �붯̬���������ʱ����
        ///    <para>* ��λ������</para>
        /// </summary>
        [CustomerField("CheckInterval")]
        public int CheckInterval;
    }
}