using KJFramework.Messages.Attributes;
using KJFramework.Net.Transaction.Identities;
using KJFramework.Net.Transaction.Messages;

namespace KJFramework.Data.Synchronization.Messages
{
    /// <summary>
    ///     订阅请求消息
    /// </summary>
    public class SubscribeRequestMessage : BaseMessage
    {
        #region Constructor

        /// <summary>
        ///     订阅请求消息
        /// </summary>
        public SubscribeRequestMessage()
        {
            MessageIdentity = new MessageIdentity
                                  {
                                      ProtocolId = 0,
                                      ServiceId = 0,
                                      DetailsId = 0
                                  };
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置分组
        /// </summary>
        [IntellectProperty(10, IsRequire = true)]
        public string Catalog { get; set; }

        #endregion
    }
}