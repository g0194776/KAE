using KJFramework.Messages.Attributes;
using KJFramework.Net.Channels.Identities;
using KJFramework.Net.Transaction.Messages;

namespace KJFramework.Data.Synchronization.Messages
{
    /// <summary>
    ///     同步数据应答消息
    /// </summary>
    public class SyncDataResponseMessage : BaseMessage
    {
        #region Constructor

        /// <summary>
        ///     同步数据请求消息
        /// </summary>
        public SyncDataResponseMessage()
        {
            MessageIdentity = new MessageIdentity
                                  {
                                      ProtocolId = 2,
                                      ServiceId = 0,
                                      DetailsId = 1
                                  };
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置错误编号
        /// </summary>
        [IntellectProperty(10)]
        public byte ErrorId { get; set; }

        #endregion
    }
}
