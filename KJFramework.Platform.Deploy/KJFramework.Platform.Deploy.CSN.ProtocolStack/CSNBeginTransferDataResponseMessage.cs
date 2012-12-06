using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.CSN.ProtocolStack
{
    /// <summary>
    ///     ��ʼ�ְ����������Ϣ
    /// </summary>
    public class CSNBeginTransferDataResponseMessage : CSNMessage
    {
        #region Constructor

        /// <summary>
        ///     ��ʼ�ְ����������Ϣ
        /// </summary>
        public CSNBeginTransferDataResponseMessage()
        {
            Header.ProtocolId = 9;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ�����ñ��ηְ����漰���������к�
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public int SerialNumber { get; set; }
        /// <summary>
        ///     ��ȡ������һ��ֵ����ֵ��ʾ�˶Է��Ƿ���ܴ˴ηְ�����ĻỰ����
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public bool IsAccept { get; set; }

        #endregion
    }
}