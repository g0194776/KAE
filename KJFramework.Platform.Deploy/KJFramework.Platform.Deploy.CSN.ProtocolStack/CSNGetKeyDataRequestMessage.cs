using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.CSN.ProtocolStack
{
    /// <summary>
    ///     ��ȡ�ؼ�����������������Ϣ
    /// </summary>
    public class CSNGetKeyDataRequestMessage : CSNMessage
    {
        #region Constructor

        /// <summary>
        ///     ��ȡ�ؼ�����������������Ϣ
        /// </summary>
        public CSNGetKeyDataRequestMessage()
        {
            Header.ProtocolId = 4;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ���������ݿ�����
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string DatabaseName { get; set; }
        /// <summary>
        ///     ��ȡ���������ݱ�����
        ///     <para>* ���ֶ�֧�ֶ��ͬʱ��ѯ���ֶ�ֵ���շֺŷָ���</para>
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public string TableName { get; set; }
        /// <summary>
        ///     ��ȡ���������ݱ������Ƽ���
        /// </summary>
        [IntellectProperty(13, IsRequire = true)]
        public string[] ColumnName { get; set; }
        /// <summary>
        ///     ��ȡ������Ҫ��ѯ�Ĺؼ����ݼ���
        /// </summary>
        [IntellectProperty(14, IsRequire = true)]
        public string[] SearchKey { get; set; }

        #endregion
    }
}