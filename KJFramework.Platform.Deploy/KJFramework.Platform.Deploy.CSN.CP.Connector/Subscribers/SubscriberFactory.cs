using System;
using KJFramework.Logger;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Enums;
using KJFramework.Platform.Deploy.CSN.ProtocolStack;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector.Subscribers
{
    /// <summary>
    ///     订阅者工厂
    /// </summary>
    public static class SubscriberFactory
    {
        #region Methods

        /// <summary>
        ///     创建一个订阅者
        /// </summary>
        /// <param name="subscriberTypes">订阅者类型</param>
        /// <param name="requestMessage">订阅者注册CSN的请求消息</param>
        /// <param name="channelId">订阅者的网络通道编号</param>
        /// <returns>返回创建后的订阅者</returns>
        public static IConfigSubscriber Create(SubscriberTypes subscriberTypes, CSNRegistRequestMessage requestMessage, Guid channelId)
        {
            try
            {
                if (requestMessage == null)
                {
                    throw new ArgumentNullException("requestMessage");
                }
                //check enum
                switch (subscriberTypes)
                {
                    case SubscriberTypes.Database:
                    case SubscriberTypes.Cache:
                    #region Create db subscriber

                    IConfigSubscriber dbConfigSubscriber = new DBConfigSubscriber(requestMessage.Header.ServiceKey, channelId)
                                                                                    {
                                                                                        NeedUpdate = requestMessage.NeedUpdate
                                                                                    };
                    return dbConfigSubscriber;

                    #endregion
                }
                return null;
            }
            catch (System.Exception e)
            {
                Logs.Logger.Log(e);
                throw;
            }
        }

        #endregion
    }
}