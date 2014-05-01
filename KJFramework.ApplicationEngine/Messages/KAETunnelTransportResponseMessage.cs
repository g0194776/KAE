using KJFramework.Messages.Attributes;

namespace KJFramework.ApplicationEngine.Messages
{
    /// <summary>
    ///     KAE隧道传递应答消息
    /// </summary>
    public class KAETunnelTransportResponseMessage : KAEResponseMessage
    {
        #region Members.

        /// <summary>
        ///     获取或设置返回的数据
        /// </summary>
        [IntellectProperty(12)]
        public byte[] Data { get; set; }

        #endregion
    }
}