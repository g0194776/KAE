using KJFramework.Net.Transaction.Messages;

namespace KJFramework.ApplicationEngine.Messages
{
    /// <summary>
    ///     基于智能对象协议的KAE业务请求消息
    /// </summary>
    public abstract class KAERequestMessage : BaseMessage
    {
        #region Methods.

        /// <summary>
        ///     创建一个跟当前协议相同的应答消息
        /// </summary>
        /// <returns>返回应答消息</returns>
        public abstract KAEResponseMessage CreateResponseMessage();


        #endregion
    }
}