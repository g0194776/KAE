using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Client.ProtocolStack
{
    /// <summary>
    ///     获取核心服务信息请求消息
    /// </summary>
    public class ClientGetCoreServiceRequestMessage : ClientMessage
    {
        #region Constructor

        /// <summary>
        ///     获取核心服务信息请求消息
        /// </summary>
        public ClientGetCoreServiceRequestMessage()
        {
            Header.ProtocolId = 16;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置要获取的核心服务分类
        ///     <para>* SMC or DSN or *SERVICE*</para>
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string Category { get; set; }

        #endregion
    }
}