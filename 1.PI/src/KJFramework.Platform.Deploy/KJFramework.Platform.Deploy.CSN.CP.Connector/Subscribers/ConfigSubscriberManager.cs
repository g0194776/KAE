using System;
using System.Collections.Generic;
using KJFramework.Logger;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Enums;
using KJFramework.Platform.Deploy.CSN.ProtocolStack;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector.Subscribers
{
    /// <summary>
    ///     配置订阅者管理器
    /// </summary>
    public class ConfigSubscriberManager
    {
        #region Members

        private static Dictionary<string, IConfigSubscriber> _subscribers = new Dictionary<string, IConfigSubscriber>();

        #endregion

        #region Methods

        /// <summary>
        ///     注册一个配置订阅者
        /// </summary>
        /// <param name="subscriberTypes">订阅者类型</param>
        /// <param name="message">订阅者注册CSN的请求消息</param>
        /// <param name="channelId">订阅者关联的网络通道编号</param>
        public static IConfigSubscriber Regist(SubscriberTypes subscriberTypes, CSNRegistRequestMessage message, Guid channelId)
        {
            try
            {
                IConfigSubscriber subscriber = SubscriberFactory.Create(subscriberTypes, message, channelId);
                if (subscriber != null)
                {
                    _subscribers[subscriber.SubscriberKey] = subscriber;
                }
                return subscriber;
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
                return null;
            }
        }

        /// <summary>
        ///     移除具有指定唯一序列值的订阅者
        /// </summary>
        /// <param name="subscriberKey">订阅者唯一序列值</param>
        public static void UnRegist(string subscriberKey)
        {
            if (string.IsNullOrEmpty(subscriberKey))
            {
                return;
            }
            _subscribers.Remove(subscriberKey);
        }

        /// <summary>
        ///     获取具有指定唯一序列值的订阅者
        /// </summary>
        /// <param name="subscriberKey">唯一序列值</param>
        /// <returns>返回订阅者</returns>
        public static IConfigSubscriber GetSubscriber(string subscriberKey)
        {
            IConfigSubscriber subscriber;
            return _subscribers.TryGetValue(subscriberKey, out subscriber) ? subscriber : null;
        }

        /// <summary>
        ///     获取具有指定网络通道编号的订阅者
        /// </summary>
        /// <param name="channelId">网络通道编号</param>
        /// <returns>返回订阅者</returns>
        public static IConfigSubscriber GetSubscriber(Guid channelId)
        {
            try
            {
                foreach (KeyValuePair<string, IConfigSubscriber> pair in _subscribers)
                {
                    if (pair.Value.ChannelId == channelId)
                    {
                        return pair.Value;
                    }
                }
                return null;
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
                return null;
            }
        }

        #endregion
    }
}