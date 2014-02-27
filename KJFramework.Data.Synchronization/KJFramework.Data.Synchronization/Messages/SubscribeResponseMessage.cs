using System;
using KJFramework.Data.Synchronization.Enums;
using KJFramework.Data.Synchronization.Policies;
using KJFramework.Messages.Attributes;
using KJFramework.Net.Channels.Identities;
using KJFramework.Net.Transaction.Messages;

namespace KJFramework.Data.Synchronization.Messages
{
    /// <summary>
    ///     订阅应答消息
    /// </summary>
    public class SubscribeResponseMessage : BaseMessage
    {
        #region Constructor

        /// <summary>
        ///     订阅请求消息
        /// </summary>
        public SubscribeResponseMessage()
        {
            MessageIdentity = new MessageIdentity
                                  {
                                      ProtocolId = 0,
                                      ServiceId = 0,
                                      DetailsId = 1
                                  };
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置分组
        /// </summary>
        [IntellectProperty(10, IsRequire = true)]
        public PublisherPolicy Policy { get; set; }
        /// <summary>
        ///     获取或设置分组
        /// </summary>
        [IntellectProperty(11)]
        public SubscribeResult Result { get; set; }
        /// <summary>
        ///     获取或设置订阅者唯一标示
        /// </summary>
        [IntellectProperty(12)]
        public Guid? Id { get; set; }

        #endregion
    }
}