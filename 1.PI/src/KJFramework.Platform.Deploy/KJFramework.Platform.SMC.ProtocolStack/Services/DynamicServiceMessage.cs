using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Platform.Deploy.SMC.ProtocolStack.Services
{
    /// <summary>
    ///     动态服务消息
    ///     <para>* 建议派生类的智能属性编号从10后开始使用。</para>
    /// </summary>
    public class DynamicServiceMessage : IntellectObject
    {
        #region Constructor

        /// <summary>
        ///     动态服务消息
        ///     <para>* 建议派生类的智能属性编号从10后开始使用。</para>
        /// </summary>
        public DynamicServiceMessage()
        {
            Header = new DynamicServiceMessageHeader();
        }

        #endregion

        #region Members

        [IntellectProperty(0, IsRequire = true)]
        public DynamicServiceMessageHeader Header { get; set; }

        #endregion
    }
}