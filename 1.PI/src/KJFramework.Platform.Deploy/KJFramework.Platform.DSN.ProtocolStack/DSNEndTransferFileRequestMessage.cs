using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.DSN.ProtocolStack
{
    /// <summary>
    ///     ���������ļ�������Ϣ
    /// </summary>
    public class DSNEndTransferFileRequestMessage : DSNMessage
    {
        #region Constructor

        /// <summary>
        ///     ���������ļ�������Ϣ
        /// </summary>
        public DSNEndTransferFileRequestMessage()
        {
            Header.ProtocolId = 3;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ��������������
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string RequestToken { get; set; }

        #endregion
    }
}