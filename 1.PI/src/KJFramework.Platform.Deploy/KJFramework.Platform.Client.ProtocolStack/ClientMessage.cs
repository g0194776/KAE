using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Platform.Client.ProtocolStack
{
    /// <summary>
    ///     �ͻ�����Ϣ
    ///     <para>* ������������������Ա�Ŵ�10��ʼʹ�á�</para>
    /// </summary>
    public class ClientMessage : IntellectObject
    {
        #region Constructor

        /// <summary>
        ///     �ͻ�����Ϣ
        ///     <para>* ������������������Ա�Ŵ�10��ʼʹ�á�</para>
        /// </summary>
        public ClientMessage()
        {
            Header = new ClientMessageHeader();
        }

        #endregion

        #region Members

        [IntellectProperty(0, IsRequire = true)]
        public ClientMessageHeader Header { get; set; }

        #endregion
    }
}