using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.CSN.ProtocolStack
{
    /// <summary>
    ///     �������֪ͨ������Ϣ
    ///     <para>* �˰�ͨ����Ҫ�ְ����ʹ�����֮ǰ�ĳ������֡�</para>
    /// </summary>
    public class CSNMultiPackageDataNotifyRequestMessage : CSNMessage
    {
        #region Constructor

        /// <summary>
        ///     �������֪ͨ������Ϣ
        ///     <para>* �˰�ͨ����Ҫ�ְ����ʹ�����֮ǰ�ĳ������֡�</para>
        /// </summary>
        public CSNMultiPackageDataNotifyRequestMessage()
        {
            Header.ProtocolId = 6;
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

        #endregion
    }
}