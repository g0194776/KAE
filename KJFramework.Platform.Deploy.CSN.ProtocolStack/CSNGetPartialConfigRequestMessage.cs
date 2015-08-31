using KJFramework.Messages.Attributes;
using KJFramework.Net.Identities;
using KJFramework.Net.Transaction.Messages;

namespace KJFramework.Platform.Deploy.CSN.ProtocolStack
{
    /// <summary>
    ///     ��ȡ����������Ϣ������Ϣ
    /// </summary>
    public class CSNGetPartialConfigRequestMessage : BaseMessage
    {
        #region Constructor

        /// <summary>
        ///     ��ȡ����������Ϣ������Ϣ
        /// </summary>
        public CSNGetPartialConfigRequestMessage()
        {
            MessageIdentity = new MessageIdentity { ProtocolId = 0, ServiceId = 4, DetailsId = 0 };
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ���������ùؼ�����
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string Key { get; set; }

        #endregion
    }
}