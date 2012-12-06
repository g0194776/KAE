using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.DSN.ProtocolStack
{
    /// <summary>
    ///     ��ʼ�����ļ���������Ϣ
    /// </summary>
    public class DSNBeginTransferFileRequestMessage : DSNMessage
    {
        #region Constructor

        /// <summary>
        ///     ��ʼ�����ļ���������Ϣ
        /// </summary>
        public DSNBeginTransferFileRequestMessage()
        {
            Header.ProtocolId = 0;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ��������������
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string RequestToken { get; set; }
        /// <summary>
        ///     ��ȡ�������ܹ��ְ���Ŀ
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public int TotalPacketCount { get; set; }
        /// <summary>
        ///     ��ȡ�����÷�����
        /// </summary>
        [IntellectProperty(13, IsRequire = true)]
        public string ServiceName { get; set; }
        /// <summary>
        ///     ��ȡ�����÷������
        /// </summary>
        [IntellectProperty(14, IsRequire = false)]
        public string Name { get; set; }
        /// <summary>
        ///     ��ȡ�����÷���汾
        /// </summary>
        [IntellectProperty(15, IsRequire = true)]
        public string Version { get; set; }
        /// <summary>
        ///     ��ȡ�����÷�������
        /// </summary>
        [IntellectProperty(16, IsRequire = false)]
        public string Description { get; set; }

        #endregion
    }
}