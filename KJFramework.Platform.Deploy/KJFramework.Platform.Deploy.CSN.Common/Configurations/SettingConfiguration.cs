using KJFramework.Attribute;

namespace KJFramework.Platform.Deploy.CSN.Common.Configurations
{
    /// <summary>
    ///   KJFramework.ServiceModel������
    /// </summary>
    public sealed class SettingConfiguration
    {
        /// <summary>
        ///   CSN���ŵĶ˿ں�
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
        ///     �������ʱ�����
        /// </summary>
        [CustomerField("CacheTimeoutCheckInterval")]
        public int CacheTimeoutCheckInterval;
        /// <summary>
        ///     ���������ʱ��
        /// </summary>
        [CustomerField("CacheLiveTime")]
        public string CacheLiveTime;
        /// <summary>
        ///     ������ݶδ�С
        ///     <para>* ���ֶ����ڷְ����ݵĴ�С���ж�</para>
        /// </summary>
        [CustomerField("MaxDataChunkSize")]
        public int MaxDataChunkSize;
    }
}