using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.CSN.ProtocolStack
{
    /// <summary>
    ///     CSN注册回馈消息
    /// </summary>
    public class CSNRegistResponseMessage : CSNMessage
    {
        #region Constructor

        /// <summary>
        ///     CSN注册回馈消息
        /// </summary>
        public CSNRegistResponseMessage()
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

        #endregion
    }
}