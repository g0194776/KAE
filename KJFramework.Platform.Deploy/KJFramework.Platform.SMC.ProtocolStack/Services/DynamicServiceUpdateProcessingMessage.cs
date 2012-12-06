using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.SMC.ProtocolStack.Services
{
    /// <summary>
    ///     ��̬������½���֪ͨ��Ϣ
    /// </summary>
    public class DynamicServiceUpdateProcessingMessage : DynamicServiceMessage
    {
        #region Constructor

        /// <summary>
        ///     ��̬������½���֪ͨ��Ϣ
        /// </summary>
        public DynamicServiceUpdateProcessingMessage()
        {
            Header.ProtocolId = 8;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ�����÷�������
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string ServiceName { get; set; }
        /// <summary>
        ///     ��ȡ�������������
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public string ComponentName { get; set; }
        /// <summary>
        ///     ��ȡ�����ø��½���
        /// </summary>
        [IntellectProperty(13, IsRequire = false)]
        public string Content { get; set; }

        #endregion
    }
}