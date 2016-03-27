using KJFramework.Messages.Attributes;
using KJFramework.Platform.Deploy.Metadata.Objects;

namespace KJFramework.Platform.Client.ProtocolStack
{
    /// <summary>
    ///     ��ȡ���ķ�����Ϣ������Ϣ
    /// </summary>
    public class ClientGetCoreServiceResponseMessage : ClientMessage
    {
        #region Constructor

        /// <summary>
        ///     ��ȡ���ķ�����Ϣ������Ϣ
        /// </summary>
        public ClientGetCoreServiceResponseMessage()
        {
            Header.ProtocolId = 17;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ�����ú��ķ�����Ϣ����
        /// </summary>
        [IntellectProperty(11, IsRequire = false)]
        public CoreServiceItem[] Items { get; set; }

        #endregion
    }
}