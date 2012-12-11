using KJFramework.Messages.Attributes;
using KJFramework.Net.Transaction.Identities;
using KJFramework.Net.Transaction.Messages;
using KJFramework.Platform.Deploy.Metadata.Objects;

namespace KJFramework.Platform.Deploy.CSN.ProtocolStack
{
    /// <summary>
    ///     ��ȡ���ñ������Ϣ
    /// </summary>
    public class CSNGetDataTableResponseMessage : BaseMessage
    {
        #region Constructor

        /// <summary>
        ///     ��ȡ���ñ������Ϣ
        /// </summary>
        public CSNGetDataTableResponseMessage()
        {
            MessageIdentity = new MessageIdentity { ProtocolId = 0, ServiceId = 2, DetailsId = 1 };
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ���������ݿ����
        /// </summary>
        [IntellectProperty(11, IsRequire = false)]
        public DataTable[] Tables { get; set; }
        /// <summary>
        ///     ��ȡ��������������Ϣ
        /// </summary>
        [IntellectProperty(12, IsRequire = false)]
        public string LastError { get; set; }

        #endregion
    }
}