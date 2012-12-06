using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.CSN.ProtocolStack
{
    /// <summary>
    ///     ������������������Ϣ
    /// </summary>
    public class CSNEndTransferDataRequestMessage : CSNMessage
    {
        #region Constructor

        /// <summary>
        ///     ������������������Ϣ
        /// </summary>
        public CSNEndTransferDataRequestMessage()
        {
            Header.ProtocolId = 11;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ�����ñ��ηְ����漰���������к�
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public int SerialNumber { get; set; }

        #endregion
    }
}