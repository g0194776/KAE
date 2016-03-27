using KJFramework.Attribute;

namespace KJFramework.Platform.Deploy.Maintenance.Configurations
{
    /// <summary>
    ///   KJFramework.ServiceModel������
    /// </summary>
    public sealed class SettingConfiguration
    {
        /// <summary>
        ///   �����ַ
        /// </summary>
        [CustomerField("ManageAddress")]
        public string ManageAddress;
        /// <summary>
        ///     ����˿�
        /// </summary>
        [CustomerField("ManagePort")]
        public int ManagePort;
        /// <summary>
        ///   ��������ĵ�����ʱ����
        ///    <para>* ��λ������</para>
        /// </summary>
        [CustomerField("HeartBeatInterval")]
        public int HeartBeatInterval;
        /// <summary>
        ///     �������ӵ�ʱ����
        ///    <para>* ��λ������</para>
        /// </summary>
        [CustomerField("ReconnectInterval")]
        public int ReconnectInterval;
    }
}