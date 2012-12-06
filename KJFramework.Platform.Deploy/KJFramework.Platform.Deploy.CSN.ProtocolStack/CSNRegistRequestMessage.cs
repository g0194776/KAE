using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.CSN.ProtocolStack
{
    /// <summary>
    ///     CSN注册请求消息
    /// </summary>
    public class CSNRegistRequestMessage : CSNMessage
    {
        #region Constructor

        /// <summary>
        ///     CSN注册请求消息
        /// </summary>
        public CSNRegistRequestMessage()
        {
            Header.ProtocolId = 0;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置一个值，该值标示了当前订阅者是否需要反向更新配置的能力
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public bool NeedUpdate { get; set; }

        #endregion
    }
}