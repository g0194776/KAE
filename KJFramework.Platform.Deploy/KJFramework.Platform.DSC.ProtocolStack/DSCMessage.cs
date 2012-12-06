using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;
using KJFramework.Platform.DSC.ProtocolStack;

namespace KJFramework.Platform.Deploy.DSC.ProtocolStack
{
    /// <summary>
    ///     服务中心消息
    ///     <para>* 建议派生类的智能属性编号从10后开始使用。</para>
    /// </summary>
    public class DSCMessage : IntellectObject
    {
        #region Constructor

        /// <summary>
        ///     服务中心消息
        ///     <para>* 建议派生类的智能属性编号从10后开始使用。</para>
        /// </summary>
        public DSCMessage()
        {
            Header = new DSCMessageHeader();
        }

        #endregion

        #region Members

        [IntellectProperty(0, IsRequire = true)]
        public DSCMessageHeader Header { get; set; }

        #endregion
    }
}