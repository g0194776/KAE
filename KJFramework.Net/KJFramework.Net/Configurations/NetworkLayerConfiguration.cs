using System;
using KJFramework.Attribute;

namespace KJFramework.Net.Configurations
{
    /// <summary>
    ///     �����������
    /// </summary>
    public sealed class NetworkLayerConfiguration
    {
        /// <summary>
        ///     TCP������������� [BufferSize - TCP�˿ڻ�������ȡ�ֽڴ�С]
        /// </summary>
        [CustomerField("BufferSize")]
        public int BufferSize;
        /// <summary>
        ///     �������󻺳����ޣ��벻ҪС��0�����Ϊ1024�ı�����
        /// </summary>
        [CustomerField("BufferPoolSize")]
        public int BufferPoolSize;
        /// <summary>
        ///     ��Ϣͷ����
        /// </summary>
        [CustomerField("MessageHeaderLength")]
        public int MessageHeaderLength;
        /// <summary>
        ///     ��Ϣͷ����ʾ
        /// </summary>
        [CustomerField("MessageHeaderFlag")]
        public String MessageHeaderFlag;
        /// <summary>
        ///     ��Ϣͷ����ʾ����
        /// </summary>
        [CustomerField("MessageHeaderFlagLength")]
        public int MessageHeaderFlagLength;
        /// <summary>
        ///     ��Ϣͷ������ʾ
        /// </summary>
        [CustomerField("MessageHeaderEndFlag")]
        public String MessageHeaderEndFlag;
        /// <summary>
        ///     ��Ϣͷ������ʾ����
        /// </summary>
        [CustomerField("MessageHeaderEndFlagLength")]
        public int MessageHeaderEndFlagLength;
        /// <summary>
        ///     ������·��
        /// </summary>
        [CustomerField("MessageDealerFolder")]
        public String MessageDealerFolder;
        /// <summary>
        ///     HOOK·��
        /// </summary>
        [CustomerField("MessageHookFolder")]
        public String MessageHookFolder;
        /// <summary>
        ///     ������·��
        /// </summary>
        [CustomerField("SpyFolder")]
        public String SpyFolder;
        /// <summary>
        ///     �Ự�ַ���ģ��
        /// </summary>
        [CustomerField("BasicSessionStringTemplate")]
        public String BasicSessionStringTemplate;
        /// <summary>
        ///     �û��������
        /// </summary>
        [CustomerField("UserHreatCheckTimeSpan")]
        public int UserHreatCheckTimeSpan;
        /// <summary>
        ///     �û�������ʱ
        /// </summary>
        [CustomerField("UserHreatTimeout")]
        public int UserHreatTimeout;
        /// <summary>
        ///     �û������������
        /// </summary>
        [CustomerField("UserHreatAlertCount")]
        public int UserHreatAlertCount;
        /// <summary>
        ///     FS���������
        /// </summary>
        [CustomerField("FSHreatCheckTimeSpan")]
        public int FSHreatCheckTimeSpan;
        /// <summary>
        ///     FS������ʱ
        /// </summary>
        [CustomerField("FSHreatTimeout")]
        public int FSHreatTimeout;
        /// <summary>
        ///     FS�����������
        /// </summary>
        [CustomerField("FSHreatAlertCount")]
        public int FSHreatAlertCount;
        /// <summary>
        ///     �Ự���ڼ����
        /// </summary>
        [CustomerField("SessionExpireCheckTimeSpan")]
        public int SessionExpireCheckTimeSpan;
        /// <summary>
        ///     Ĭ�����ӳص���������
        /// </summary>
        [CustomerField("DefaultConnectionPoolConnectCount")]
        public int DefaultConnectionPoolConnectCount;
        /// <summary>
        ///     ��Խ��CPUʹ����ָ��
        /// </summary>
        [CustomerField("PredominantCpuUsage")]
        public int PredominantCpuUsage;
        /// <summary>
        ///     ��Խ���ڴ�ʹ����ָ��
        /// </summary>
        [CustomerField("PredominantMemoryUsage")]
        public int PredominantMemoryUsage;
        /// <summary>
        ///     Ĭ�ϵ�ͨ��Ⱥ���
        /// </summary>
        [CustomerField("DefaultChannelGroupLayer")]
        public int DefaultChannelGroupLayer;
        /// <summary>
        ///     Ĭ�Ͽ������Ķ�������
        /// </summary>
        [CustomerField("DefaultDecleardSize")]
        public int DefaultDecleardSize;
    }
}