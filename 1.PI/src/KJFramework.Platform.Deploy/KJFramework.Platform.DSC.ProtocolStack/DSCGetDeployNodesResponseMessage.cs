using KJFramework.Messages.Attributes;
using KJFramework.Platform.Deploy.Metadata.Objects;

namespace KJFramework.Platform.Deploy.DSC.ProtocolStack
{
    /// <summary>
    ///     ��ȡ������ע��Ĳ���ڵ���Ϣ������Ϣ
    /// </summary>
    public class DSCGetDeployNodesResponseMessage : DSCMessage
    {
        #region Constructor

        /// <summary>
        ///     ��ȡ������ע��Ĳ���ڵ���Ϣ������Ϣ
        /// </summary>
        public DSCGetDeployNodesResponseMessage()
        {
            Header.ProtocolId = 20;
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