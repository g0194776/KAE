using KJFramework.Messages.Attributes;
using KJFramework.Platform.Deploy.Metadata.Objects;

namespace KJFramework.Platform.Deploy.DSN.ProtocolStack
{
    /// <summary>
    ///     ��ȡ���ط�����ϸ��Ϣ������Ϣ
    /// </summary>
    public class DSNGetLocalServiceInfomationResponseMessage : DSNMessage
    {
        #region Constructor

        /// <summary>
        ///     ��ȡ���ط�����ϸ��Ϣ������Ϣ
        /// </summary>
        public DSNGetLocalServiceInfomationResponseMessage()
        {
            Header.ProtocolId = 16;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ�����û�����
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string MachineName { get; set; }
        /// <summary>
        ///     ��ȡ�����÷�����ϸ��Ϣ����
        /// </summary>
        [IntellectProperty(12, IsRequire = false)]
        public ServiceInfoItem[] Services { get; set; }
        /// <summary>
        ///     ��ȡ��������������Ϣ
        /// </summary>
        [IntellectProperty(13, IsRequire = false)]
        public string LastError { get; set; }

        #endregion
    }
}