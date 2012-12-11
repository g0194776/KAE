using KJFramework.Messages.Attributes;
using KJFramework.Net.Transaction.Identities;
using KJFramework.Net.Transaction.Messages;
using KJFramework.Platform.Deploy.Metadata.Objects;

namespace KJFramework.Platform.Deploy.CSN.ProtocolStack
{
    /// <summary>
    ///     ��ȡ�ؼ����������û�����Ϣ
    /// </summary>
    public class CSNGetKeyDataResponseMessage : BaseMessage
    {
        #region Constructor

        /// <summary>
        ///     ��ȡ�ؼ����������û�����Ϣ
        /// </summary>
        public CSNGetKeyDataResponseMessage()
        {
            MessageIdentity = new MessageIdentity { ProtocolId = 0, ServiceId = 3, DetailsId = 1 };
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ�����ò�ѯ������ݼ���
        /// </summary>
        [IntellectProperty(11, IsRequire = false)]
        public KeyValueItem[] Items { get; set; }
        /// <summary>
        ///     ��ȡ���������Ĵ�����Ϣ
        /// </summary>
        [IntellectProperty(12, IsRequire = false)]
        public string LastError { get; set; }

        #endregion
    }
}