using KJFramework.Messages.Attributes;
using KJFramework.Platform.Deploy.CSN.ProtocolStack.Enums;

namespace KJFramework.Platform.Deploy.CSN.ProtocolStack
{
    /// <summary>
    ///     ��ʼ�ְ�����������Ϣ
    /// </summary>
    public class CSNBeginTransferDataRequestMessage :   CSNMessage
    {
        #region Constructor

        /// <summary>
        ///     ��ʼ�ְ�����������Ϣ
        /// </summary>
        public CSNBeginTransferDataRequestMessage()
        {
            Header.ProtocolId = 8;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ��������һ��������Ϣ������ĻỰ���
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public int PreviousSessionId { get; set; }
        /// <summary>
        ///     ��ȡ�����ñ��ηְ����漰���������к�
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public int SerialNumber { get; set; }
        /// <summary>
        ///     ��ȡ�������ܹ��ķְ���Ŀ
        /// </summary>
        [IntellectProperty(13, IsRequire = true)]
        public int TotalPackageCount { get; set; }
        /// <summary>
        ///     ��ȡ�������ܹ������ݴ�С
        /// </summary>
        [IntellectProperty(14, IsRequire = true)]
        public int TotalDataLength { get; set; }
        /// <summary>
        ///     ��ȡ���������������������
        /// </summary>
        [IntellectProperty(15, IsRequire = true)]
        public ConfigTypes ConfigType { get; set; }

        #endregion
    }
}