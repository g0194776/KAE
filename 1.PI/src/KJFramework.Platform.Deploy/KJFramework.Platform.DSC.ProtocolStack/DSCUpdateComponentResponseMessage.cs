using KJFramework.Messages.Attributes;
using KJFramework.Platform.Deploy.Metadata.Objects;

namespace KJFramework.Platform.Deploy.DSC.ProtocolStack
{
    /// <summary>
    ///     �������������Ϣ
    /// </summary>
    public class DSCUpdateComponentResponseMessage : DSCMessage
    {
        #region Constructor

        /// <summary>
        ///     �������������Ϣ
        /// </summary>
        public DSCUpdateComponentResponseMessage()
        {
            Header.ProtocolId = 13;
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
        /// <summary>
        ///     ��ȡ�����û�����
        /// </summary>
        [IntellectProperty(15, IsRequire = true)]
        public string MachineName { get; set; }

        #endregion
    }
}