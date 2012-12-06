using KJFramework.Attribute;

namespace KJFramework.ServiceModel.Configurations
{
    /// <summary>
    ///   KJFramework.ServiceModel������
    /// </summary>
    public sealed class SettingConfiguration
    {
        /// <summary>
        ///   �������ĵȴ�����ػ�����
        /// </summary>
        [CustomerField("RequestCenterWaitObjectPoolCount")]
        public int RequestCenterWaitObjectPoolCount;
        /// <summary>
        ///    ������Ϣ�ػ�����
        /// </summary>
        [CustomerField("RequestServiceMessagePoolCount")]
        public int RequestServiceMessagePoolCount;
        /// <summary>
        ///   ��Ӧ��Ϣ�ػ�����
        /// </summary>
        [CustomerField("ResponseServiceMessagePoolCount")]
        public int ResponseServiceMessagePoolCount;
        /// <summary>
        ///   �������ĵȴ�����ػ�����
        /// </summary>
        [CustomerField("ServiceCallContextPoolCount")]
        public int ServiceCallContextPoolCount;
        /// <summary>
        ///     ������Լ����ĳػ�����
        /// </summary>
        [CustomerField("ServiceProviderObjectPoolCount")]
        public int ServiceProviderObjectPoolCount;
        /// <summary>
        ///     ����ͻ����ŵ������ʾ
        /// </summary>
        [CustomerField("AllowClientCache")]
        public bool AllowClientCache;
    }
}