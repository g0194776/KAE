using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.CSN.ProtocolStack
{
    /// <summary>
    ///     ��ȡ���ñ�������Ϣ
    /// </summary>
    public class CSNGetDataTableRequestMessage : CSNMessage
    {
        #region Constructor

        /// <summary>
        ///     ��ȡ���ñ�������Ϣ
        /// </summary>
        public CSNGetDataTableRequestMessage()
        {
            Header.ProtocolId = 2;
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

        #endregion
    }
}