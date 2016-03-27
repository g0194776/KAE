using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.DSN.ProtocolStack
{
    /// <summary>
    ///     ���������ļ�������Ϣ
    /// </summary>
    public class DSNEndTransferFileResponseMessage : DSNMessage
    {
        #region Constructor

        /// <summary>
        ///     ���������ļ�������Ϣ
        /// </summary>
        public DSNEndTransferFileResponseMessage()
        {
            Header.ProtocolId = 4;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ��������������
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string RequestToken { get; set; }
        /// <summary>
        ///     ��ȡ������һ��ֵ����ֵ��ʾ�˵�ǰ�Ƿ��Ѿ����ݳɹ�
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public bool IsDone { get; set; }
        /// <summary>
        ///     ��ȡ�����õ�ǰ���ݰ����
        /// </summary>
        [IntellectProperty(13, IsRequire = false)]
        public int[] LosePackets { get; set; }

        #endregion
    }
}