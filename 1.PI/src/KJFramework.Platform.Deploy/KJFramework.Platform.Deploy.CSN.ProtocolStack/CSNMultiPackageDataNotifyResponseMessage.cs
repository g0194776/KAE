using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.CSN.ProtocolStack
{
    /// <summary>
    ///     �������֪ͨ������Ϣ
    ///     <para>* �˰�ͨ����Ҫ�ְ����ʹ�����֮ǰ�ĳ������֡�</para>
    /// </summary>
    public class CSNMultiPackageDataNotifyResponseMessage : CSNMessage
    {
        #region Constructor

        /// <summary>
        ///     �������֪ͨ������Ϣ
        ///     <para>* �˰�ͨ����Ҫ�ְ����ʹ�����֮ǰ�ĳ������֡�</para>
        /// </summary>
        public CSNMultiPackageDataNotifyResponseMessage()
        {
            Header.ProtocolId = 7;
        }
            
        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ�����ñ��ηְ����漰���������к�
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public int SerialNumber { get; set; }

        #endregion
    }
}