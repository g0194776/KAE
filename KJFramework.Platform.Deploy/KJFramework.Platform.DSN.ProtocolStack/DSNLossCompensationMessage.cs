using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.DSN.ProtocolStack
{
    /// <summary>
    ///     ����������Ϣ
    /// </summary>
    public class DSNLossCompensationMessage : DSNMessage
    {
        #region Constructor

        /// <summary>
        ///     ����������Ϣ
        /// </summary>
        public DSNLossCompensationMessage()
        {
            Header.ProtocolId = 5;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ��������������
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string RequestToken { get; set; }
        /// <summary>
        ///     ��ȡ�����õ�ǰ���ݰ����
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public int CurrentPackageId { get; set; }
        /// <summary>
        ///     ��ȡ�����õ�ǰ���ݰ�����
        /// </summary>
        [IntellectProperty(13, IsRequire = true)]
        public byte[] Data { get; set; }

        #endregion
    }
}