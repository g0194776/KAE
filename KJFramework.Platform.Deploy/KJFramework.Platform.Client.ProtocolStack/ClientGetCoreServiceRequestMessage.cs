using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Client.ProtocolStack
{
    /// <summary>
    ///     ��ȡ���ķ�����Ϣ������Ϣ
    /// </summary>
    public class ClientGetCoreServiceRequestMessage : ClientMessage
    {
        #region Constructor

        /// <summary>
        ///     ��ȡ���ķ�����Ϣ������Ϣ
        /// </summary>
        public ClientGetCoreServiceRequestMessage()
        {
            Header.ProtocolId = 16;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ������Ҫ��ȡ�ĺ��ķ������
        ///     <para>* SMC or DSN or *SERVICE*</para>
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string Category { get; set; }

        #endregion
    }
}