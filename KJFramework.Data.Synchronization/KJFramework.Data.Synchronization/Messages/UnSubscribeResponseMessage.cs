using KJFramework.Net.Transaction.Identities;
using KJFramework.Net.Transaction.Messages;

namespace KJFramework.Data.Synchronization.Messages
{
    /// <summary>
    ///     取消订阅请求消息
    /// </summary>
    public class UnSubscribeResponseMessage : BaseMessage
    {
        #region Constructor

        /// <summary>
        ///     取消订阅请求消息
        /// </summary>
        public UnSubscribeResponseMessage()
        {
            MessageIdentity = new MessageIdentity
                                  {
                                      ProtocolId = 1,
                                      ServiceId = 0,
                                      DetailsId = 1
                                  };
        }

        #endregion

        #region Members



        #endregion
    }
}