using System;
using KJFramework.Data.Synchronization.Enums;
using KJFramework.Messages.Attributes;
using KJFramework.Net.Transaction.Identities;
using KJFramework.Net.Transaction.Messages;

namespace KJFramework.Data.Synchronization.Messages
{
    /// <summary>
    ///     取消订阅请求消息
    /// </summary>
    public class UnSubscribeRequestMessage : BaseMessage
    {
        #region Constructor

        /// <summary>
        ///     取消订阅请求消息
        /// </summary>
        public UnSubscribeRequestMessage()
        {
            MessageIdentity = new MessageIdentity
                                  {
                                      ProtocolId = 1,
                                      ServiceId = 0,
                                      DetailsId = 0
                                  };
        }

        #endregion

        #region Members

        /// <summary>
        ///     取消订阅模式
        /// </summary>
        [IntellectProperty(10)]
        public UnSubscribeMode Mode { get; set; }
        /// <summary>
        ///     获取或设置分类名称
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string Catelog { get; set; }
        /// <summary>
        ///     获取或设置订阅者唯一标示
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public Guid Id { get; set; }

        #endregion
    }
}