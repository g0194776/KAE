using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;
using KJFramework.Platform.Deploy.CSN.Common;

namespace KJFramework.Platform.Deploy.CSN.ProtocolStack
{
    /// <summary>
    ///     配置站节点消息
    ///     <para>* 建议派生类的智能属性编号从10后开始使用。</para>
    /// </summary>
    public class CSNMessage : IntellectObject
    {
        #region Constructor

        /// <summary>
        ///     部署节点消息
        ///     <para>* 建议派生类的智能属性编号从10后开始使用。</para>
        /// </summary>
        public CSNMessage()
        {
            Header = new CSNMessageHeader();
            Header.SessionId = SessionGenerator.Create();
        }

        #endregion

        #region Members

        [IntellectProperty(0, IsRequire = true)]
        public CSNMessageHeader Header { get; set; }

        #endregion
    }
}