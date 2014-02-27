using KJFramework.Messages.Attributes;
using KJFramework.Net.Channels.Identities;
using KJFramework.Net.Transaction.Messages;

namespace KJFramework.Platform.Deploy.CSN.ProtocolStack
{
    /// <summary>
    ///     ��ȡ����������Ϣ������Ϣ
    /// </summary>
    public class CSNGetPartialConfigResponseMessage : BaseMessage
    {
        #region Constructor

        /// <summary>
        ///     ��ȡ����������Ϣ������Ϣ
        /// </summary>
        public CSNGetPartialConfigResponseMessage()
        {
            MessageIdentity = new MessageIdentity { ProtocolId = 0, ServiceId = 4, DetailsId = 1 };
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ�����ò�ѯ������ݼ���
        /// </summary>
        [IntellectProperty(11, AllowDefaultNull = true)]
        public byte ErrorId { get; set; }
        /// <summary>
        ///     ��ȡ���������Ĵ�����Ϣ
        /// </summary>
        [IntellectProperty(12)]
        public string LastError { get; set; }
        /// <summary>
        ///     ��ȡ������������Ϣ
        /// </summary>
        [IntellectProperty(13)]
        public string Config { get; set; }

        #endregion
    }
}