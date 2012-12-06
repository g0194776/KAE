using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.DSN.ProtocolStack
{
    /// <summary>
    ///     ��ʼ�����ļ��Ļ�����Ϣ
    /// </summary>
    public class DSNBeginTransferFileResponseMessage : DSNMessage
    {
        #region Constructor

        /// <summary>
        ///     ��ʼ�����ļ��Ļ�����Ϣ
        /// </summary>
        public DSNBeginTransferFileResponseMessage()
        {
            Header.ProtocolId = 1;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ��������������
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string RequestToken { get; set; }
        /// <summary>
        ///     ��ȡ������һ��ֵ����ֵ��ʾ�˵�ǰ����ڵ��Ƿ�ͬ�⿪ʼ�����ļ�
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public bool IsAccept { get; set; }
        /// <summary>
        ///     ��ȡ�����þܾ������ļ���ԭ��
        /// </summary>
        [IntellectProperty(13, IsRequire = false)]
        public string Reason { get; set; }

        #endregion
    }
}