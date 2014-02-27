using KJFramework.Messages.Attributes;
using KJFramework.Net.Channels.Identities;
using KJFramework.Net.Transaction.Messages;

namespace KJFramework.Platform.Deploy.CSN.ProtocolStack
{
    /// <summary>
    ///     ��ȡ�ؼ�����������������Ϣ
    /// </summary>
    public class CSNGetKeyDataRequestMessage : BaseMessage
    {
        #region Constructor

        /// <summary>
        ///     ��ȡ�ؼ�����������������Ϣ
        /// </summary>
        public CSNGetKeyDataRequestMessage()
        {
            MessageIdentity = new MessageIdentity { ProtocolId = 0, ServiceId = 3, DetailsId = 0 };
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
        ///     ��ȡ������Ҫ��ѯ�Ĺؼ����ݼ���
        /// </summary>
        [IntellectProperty(13, IsRequire = true)]
        public string ServiceName { get; set; }

        //bool hasWatch;

        #endregion
    }
}