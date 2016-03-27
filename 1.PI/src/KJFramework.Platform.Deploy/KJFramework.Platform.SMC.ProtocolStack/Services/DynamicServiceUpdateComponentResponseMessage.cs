using KJFramework.Messages.Attributes;
using KJFramework.Platform.Deploy.Metadata.Objects;

namespace KJFramework.Platform.Deploy.SMC.ProtocolStack.Services
{
    /// <summary>
    ///     ��̬����������������Ϣ
    /// </summary>
    public class DynamicServiceUpdateComponentResponseMessage : DynamicServiceMessage
    {
        #region Constructor

        /// <summary>
        ///     ��̬����������������Ϣ
        /// </summary>
        public DynamicServiceUpdateComponentResponseMessage()
        {
            Header.ProtocolId = 7;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ�����÷�����
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public bool Result { get; set; }
        /// <summary>
        ///     ��ȡ�����ô�����Ϣ
        /// </summary>
        [IntellectProperty(12, IsRequire = false)]
        public string ErrorTrace { get; set; }
        /// <summary>
        ///     ��ȡ�����÷�������
        /// </summary>
        [IntellectProperty(13, IsRequire = true)]
        public string ServiceName { get; set; }
        /// <summary>
        ///     ��ȡ������������½��
        /// </summary>
        [IntellectProperty(14, IsRequire = false)]
        public ComponentUpdateResultItem[] Items { get; set; }

        #endregion
    }
}