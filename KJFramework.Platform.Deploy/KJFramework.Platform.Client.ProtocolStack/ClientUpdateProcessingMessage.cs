using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Client.ProtocolStack
{
    /// <summary>
    ///     ����֪ͨ��Ϣ
    /// </summary>
    public class ClientUpdateProcessingMessage : ClientMessage
    {
        #region Constructor

        /// <summary>
        ///     ����֪ͨ��Ϣ
        /// </summary>
        public ClientUpdateProcessingMessage()
        {
            Header.ProtocolId = 11;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ�����÷�������
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string ServiceName { get; set; }
        /// <summary>
        ///     ��ȡ�������������
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public string ComponentName { get; set; }
        /// <summary>
        ///     ��ȡ�����ø��½���
        /// </summary>
        [IntellectProperty(13, IsRequire = false)]
        public string Content { get; set; }

        #endregion
    }
}