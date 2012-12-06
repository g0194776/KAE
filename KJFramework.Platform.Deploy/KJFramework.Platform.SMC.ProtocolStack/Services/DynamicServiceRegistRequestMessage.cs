using KJFramework.Messages.Attributes;
using KJFramework.Platform.Deploy.Metadata.Objects;

namespace KJFramework.Platform.Deploy.SMC.ProtocolStack.Services
{
    /// <summary>
    ///     ��̬����ע��������Ϣ
    /// </summary>
    public class DynamicServiceRegistRequestMessage : DynamicServiceMessage
    {
        #region Constructor

        /// <summary>
        ///     ��̬����ע��������Ϣ
        /// </summary>
        public DynamicServiceRegistRequestMessage()
        {
            Header.ProtocolId = 0;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ�����÷�����
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string ServiceName { get; set; }
        /// <summary>
        ///     ��ȡ�����÷���汾
        /// </summary>
        [IntellectProperty(12, IsRequire = false)]
        public string Version { get; set; }
        /// <summary>
        ///     ��ȡ�����÷������
        /// </summary>
        [IntellectProperty(13, IsRequire = false)]
        public string Name { get; set; }
        /// <summary>
        ///     ��ȡ�����÷�������
        /// </summary>
        [IntellectProperty(14, IsRequire = false)]
        public string Description { get; set; }
        /// <summary>
        ///     ��ȡ������һ��ֵ����ֵ��ʾ�˵�ǰ��̬�����Ƿ�֧�ֳ���������ܲ���
        /// </summary>
        [IntellectProperty(15, IsRequire = true)]
        public bool SupportDomainPerformance { get; set; }
        /// <summary>
        ///     ��ȡ�����ý�������
        /// </summary>
        [IntellectProperty(16, IsRequire = true)]
        public string ProcessName { get; set; }
        /// <summary>
        ///     ��ȡ�������������
        /// </summary>
        [IntellectProperty(17, IsRequire = true)]
        public int ComponentCount { get; set; }
        /// <summary>
        ///     ��ȡ����������Ľ���״̬
        /// </summary>
        [IntellectProperty(18, IsRequire = false)]
        public ComponentDetailItem[] Items { get; set; }
        /// <summary>
        ///     ��ȡ�����õ�ǰ���ض���ǰ汾��
        /// </summary>
        [IntellectProperty(19, IsRequire = false)]
        public string ShellVersion { get; set; }

        #endregion
    }
}