using KJFramework.Messages.Attributes;
using KJFramework.Platform.Deploy.Metadata.Objects;

namespace KJFramework.Platform.Client.ProtocolStack
{
    /// <summary>
    ///     ��ȡ������ע��Ĳ���ڵ���Ϣ������Ϣ
    /// </summary>
    public class ClientGetDeployNodesResponseMessage : ClientMessage
    {
        #region Constructor

        /// <summary>
        ///     ��ȡ������ע��Ĳ���ڵ���Ϣ������Ϣ
        /// </summary>
        public ClientGetDeployNodesResponseMessage()
        {
            Header.ProtocolId = 15;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ�����ò���ڵ���Ϣ����
        /// </summary>
        [IntellectProperty(11, IsRequire = false)]
        public OwnDeployNodeItem[] Items { get; set; }

        #endregion
    }
}