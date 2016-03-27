using System;
using KJFramework.Platform.Deploy.CSN.CP.Connector.SubscribeObjs;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector.Subscribers
{
    /// <summary>
    ///     数据库配置订阅者，提供了相关的基本操作
    /// </summary>
    public class DBConfigSubscriber : ConfigSubscriber, IDBConfigSubscriber
    {
        #region Constructor

        /// <summary>
        ///     数据库配置订阅者，提供了相关的基本操作
        /// </summary>
        /// <param name="subscriberKey">订阅者唯一序列值</param>
        /// <param name="channelId">订阅者网络通道编号</param>
        public DBConfigSubscriber(string subscriberKey, Guid channelId)
            : base(subscriberKey, channelId)
        {
            _subscribeObject = new DBSubscribeObject();
        }

        #endregion
    }
}