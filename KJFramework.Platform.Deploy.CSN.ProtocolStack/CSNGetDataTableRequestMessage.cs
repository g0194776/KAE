using KJFramework.Messages.Attributes;
using KJFramework.Net.Transaction.Identities;
using KJFramework.Net.Transaction.Messages;

namespace KJFramework.Platform.Deploy.CSN.ProtocolStack
{
    /// <summary>
    ///     ��ȡ���ñ�������Ϣ
    /// </summary>
    public class CSNGetDataTableRequestMessage : BaseMessage
    {
        #region Constructor

        /// <summary>
        ///     ��ȡ���ñ�������Ϣ
        /// </summary>
        public CSNGetDataTableRequestMessage()
        {
            MessageIdentity = new MessageIdentity {ProtocolId = 0, ServiceId = 2, DetailsId = 0};
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