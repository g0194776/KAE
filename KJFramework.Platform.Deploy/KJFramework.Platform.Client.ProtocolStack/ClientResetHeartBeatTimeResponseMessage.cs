using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Client.ProtocolStack
{
    /// <summary>
    ///     �����������������Ϣ
    /// </summary>
    public class ClientResetHeartBeatTimeResponseMessage : ClientMessage
    {
        #region Constructor

        /// <summary>
        ///     �����������������Ϣ
        /// </summary>
        public ClientResetHeartBeatTimeResponseMessage()
        {
            Header.ProtocolId = 6;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ��������������ʱ��Ľ��
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public bool Result { get; set; }
        /// <summary>
        ///     ��ȡ�����û�������
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public string MachineName { get; set; }

        #endregion
    }
}