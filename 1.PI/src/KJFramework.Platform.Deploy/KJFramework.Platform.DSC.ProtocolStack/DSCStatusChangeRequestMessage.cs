using KJFramework.Messages.Attributes;
using KJFramework.Platform.Deploy.Metadata.Enums;
using KJFramework.Platform.Deploy.Metadata.Objects;

namespace KJFramework.Platform.Deploy.DSC.ProtocolStack
{
    /// <summary>
    ///     ״̬���������Ϣ
    /// </summary>
    public class DSCStatusChangeRequestMessage : DSCMessage
    {
        #region Constructor

        /// <summary>
        ///     ״̬���������Ϣ
        /// </summary>
        public DSCStatusChangeRequestMessage()
        {
            Header.ProtocolId = 8;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ������״̬���������
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public StatusChangeTypes StatusChangeType { get; set; }
        /// <summary>
        ///     ��ȡ������״̬�����
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public OwnServiceItem[] Items { get; set; }
        /// <summary>
        ///     ��ȡ�����û�����
        /// </summary>
        [IntellectProperty(13, IsRequire = true)]
        public string MachineName { get; set; }

        #endregion
    }
}