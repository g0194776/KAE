using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.CSN.ProtocolStack
{
    /// <summary>
    ///     �ְ��������ݵ������
    /// </summary>
    public class CSNTransferDataMessage :  CSNMessage
    {
        #region Constructor

        /// <summary>
        ///     �ְ��������ݵ������
        /// </summary>
        public CSNTransferDataMessage()
        {
            Header.ProtocolId = 10;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ�����ñ��ηְ����漰���������к�
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public int SerialNumber { get; set; }
        /// <summary>
        ///     ��ȡ�����ñ������ݰ��ı��
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public int PackageId { get; set; }
        /// <summary>
        ///     ��ȡ�����ñ��δ��������
        /// </summary>
        [IntellectProperty(13, IsRequire = true)]
        public byte[] Data { get; set; }

        #endregion
    }
}