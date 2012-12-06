using KJFramework.Messages.Attributes;
using KJFramework.Net.Transaction.Identities;
using KJFramework.Net.Transaction.Messages;

namespace KJFramework.Data.Synchronization.Messages
{
    public class BroadcastResponseMessage : BaseMessage
    {
        #region Constructor

        /// <summary>
        ///     广播响应消息
        /// </summary>
        public BroadcastResponseMessage()
        {
            MessageIdentity = new MessageIdentity
                                  {
                                      ProtocolId = 3,
                                      ServiceId = 0,
                                      DetailsId = 1
                                  };
        }

        /// <summary>
        ///     错误编号
        /// </summary>
        [IntellectProperty(10)]
        public byte ErrorId { get; set; }

        #endregion
    }
}