using KJFramework.Messages.Attributes;
using KJFramework.Platform.Deploy.Metadata.Objects;

namespace KJFramework.Platform.Client.ProtocolStack
{
    /// <summary>
    ///     �ͻ��˻�ȡ���з��������Ϣ������Ϣ
    /// </summary>
    public class ClientGetServicesResponseMessage : ClientMessage
    {
        #region Constructor

        /// <summary>
        ///     �ͻ��˻�ȡ���з��������Ϣ������Ϣ
        /// </summary>
        public ClientGetServicesResponseMessage()
        {
            Header.ProtocolId = 3;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ�����÷�����ϸ��Ϣ
        /// </summary>
        [IntellectProperty(11, IsRequire = false)]
        public OwnServiceItem[] Items { get; set; }
        /// <summary>
        ///     ��ȡ�����û�����
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public string MachineName { get; set; }

        #endregion
    }
}