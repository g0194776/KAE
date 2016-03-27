using KJFramework.Messages.Attributes;
using KJFramework.Platform.Deploy.Metadata.Performances;
using KJFramework.Platform.Deploy.SMC.Common.Performances;

namespace KJFramework.Platform.Deploy.SMC.ProtocolStack.Services
{
    /// <summary>
    ///     ��̬��������������Ϣ
    /// </summary>
    public class DynamicServiceHeartBeatResponseMessage : DynamicServiceMessage
    {
        #region Constructor

        /// <summary>
        ///     ��̬��������������Ϣ
        /// </summary>
        public DynamicServiceHeartBeatResponseMessage()
        {
            Header.ProtocolId = 3;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ�������������
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public bool Result { get; set; }
        /// <summary>
        ///     ��ȡ�����÷�����
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public string ServiceName { get; set; }
        /// <summary>
        ///     ��ȡ������һ��ֵ����ֵ��ʾ�˵�ǰ��̬�����Ƿ�֧��Ӧ�ó����������ָ�겶��
        /// </summary>
        [IntellectProperty(13, IsRequire = true)]
        public bool SupportDomainPerformance { get; set; }
        /// <summary>
        ///     ��ȡ�����÷�����ÿ���������
        /// </summary>
        [IntellectProperty(14, IsRequire = false)]
        public DomainPerformanceItem[] DomainItems { get; set; }
        /// <summary>
        ///     ��ȡ�����÷����е�����
        /// </summary>
        [IntellectProperty(15, IsRequire = false)]
        public ServicePerformanceItem[] PerformanceItems { get; set; }
        /// <summary>
        ///     ��ȡ���������������
        /// </summary>
        [IntellectProperty(16, IsRequire = false)]
        public ComponentHealthItem[] ComponentItems { get; set; }
        /// <summary>
        ///     ��ȡ�������������
        /// </summary>
        [IntellectProperty(17, IsRequire = false)]
        public int ComponentCount { get; set; }

        #endregion
    }
}