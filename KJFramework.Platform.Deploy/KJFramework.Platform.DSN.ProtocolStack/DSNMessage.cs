using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Platform.Deploy.DSN.ProtocolStack
{
    /// <summary>
    ///     部署节点消息
    ///     <para>* 建议派生类的智能属性编号从10后开始使用。</para>
    /// </summary>
    public class DSNMessage : IntellectObject
    {
        #region Constructor

        /// <summary>
        ///     部署节点消息
        ///     <para>* 建议派生类的智能属性编号从10后开始使用。</para>
        /// </summary>
        public DSNMessage()
        {
            Header = new DSNMessageHeader();
        }

        #endregion

        #region Members

        [IntellectProperty(0, IsRequire = true)]
        public DSNMessageHeader Header { get; set; }

        #endregion
    }
}