using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Platform.Client.ProtocolStack
{
    /// <summary>
    ///     客户端消息
    ///     <para>* 建议派生类的智能属性编号从10后开始使用。</para>
    /// </summary>
    public class ClientMessage : IntellectObject
    {
        #region Constructor

        /// <summary>
        ///     客户端消息
        ///     <para>* 建议派生类的智能属性编号从10后开始使用。</para>
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