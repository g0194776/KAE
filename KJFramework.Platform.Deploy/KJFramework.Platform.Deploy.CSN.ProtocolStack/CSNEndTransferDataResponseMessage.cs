using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.CSN.ProtocolStack
{
    /// <summary>
    ///     �����������ݻ�����
    /// </summary>
    public class CSNEndTransferDataResponseMessage : CSNMessage
    {
        #region Constructor

        /// <summary>
        ///     �����������ݻ�����
        /// </summary>
        public CSNEndTransferDataResponseMessage()
        {
            Header.ProtocolId = 12;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ�����ñ��ηְ����漰���������к�
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public int SerialNumber { get; set; }
        /// <summary>
        ///     ��ȡ������һ��ֵ����ֵ��ʾ�˵�ǰ�����Ƿ��Ѿ����
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public bool IsComplated { get; set; }
        /// <summary>
        ///     ��ȡ�����õ�ǰ�����ı�ż���
        ///     <para>* ��ֵ����IsComplated = falseʱ��Ч��</para>
        /// </summary>
        [IntellectProperty(13, IsRequire = false)]
        public int[] PackageIds { get; set; }

        #endregion
    }
}