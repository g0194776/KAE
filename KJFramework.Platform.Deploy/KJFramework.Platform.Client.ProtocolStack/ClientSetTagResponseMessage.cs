using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Client.ProtocolStack
{
    /// <summary>
    ///     �ͻ������ñ�ʶ������Ϣ
    /// </summary>
    public class ClientSetTagResponseMessage : ClientMessage
    {
        #region Constructor

        /// <summary>
        ///     �ͻ������ñ�ʶ������Ϣ
        /// </summary>
        public ClientSetTagResponseMessage()
        {
            Header.ProtocolId = 1;
        }

        #endregion

        #region Members
        
        /// <summary>
        ///     ��ȡ�����ý��
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public bool Result { get; set; }

        #endregion
    }
}