using KJFramework.Messages.Attributes;
using KJFramework.Platform.Deploy.Metadata.Uri;

namespace KJFramework.Platform.Deploy.SMC.ProtocolStack.Services
{
    /// <summary>
    ///     动态服务注册回馈消息
    /// </summary>
    public class DynamicServiceRegistResponseMessage : DynamicServiceMessage
    {
        #region Constructor

        /// <summary>
        ///     动态服务注册回馈消息
        /// </summary>
        public DynamicServiceRegistResponseMessage()
        {
            Header.ProtocolId = 1;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置注册的结果
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public bool Result { get; set; }
        /// <summary>
        ///     获取或设置期望Shell知道的相关地址列表
        /// </summary>
        [IntellectProperty(12, IsRequire = false)]
        public AddressUri[] Addresses { get; set; }

        #endregion
    }
}