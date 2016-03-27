using KJFramework.Messages.Attributes;
using KJFramework.Net.Transaction.Messages;

namespace KJFramework.ApplicationEngine.Messages
{
    /// <summary>
    ///     基于智能对象协议的KAE业务应答消息
    /// </summary>
    public class KAEResponseMessage : BaseMessage
    {
        #region Members.

        /// <summary>
        ///     获取或设置错误编号
        /// </summary>
        [IntellectProperty(10)]
        public byte ErrorId { get; set; }
        /// <summary>
        ///     获取或设置错误原因
        /// </summary>
        [IntellectProperty(11)]
        public string Reason { get; set; }

        #endregion
    }
}