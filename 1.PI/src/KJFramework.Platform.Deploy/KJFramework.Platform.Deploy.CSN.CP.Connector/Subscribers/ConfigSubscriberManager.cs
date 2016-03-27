using System;
using System.Collections.Generic;
using KJFramework.Logger;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Enums;
using KJFramework.Platform.Deploy.CSN.ProtocolStack;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector.Subscribers
{
    /// <summary>
    ///     ���ö����߹�����
    /// </summary>
    public class ConfigSubscriberManager
    {
        #region Members

        private static Dictionary<string, IConfigSubscriber> _subscribers = new Dictionary<string, IConfigSubscriber>();

        #endregion

        #region Methods

        /// <summary>
        ///     ע��һ�����ö�����
        /// </summary>
        /// <param name="subscriberTypes">����������</param>
        /// <param name="message">������ע��CSN��������Ϣ</param>
        /// <param name="channelId">�����߹���������ͨ�����</param>
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
        ///     �Ƴ�����ָ��Ψһ����ֵ�Ķ�����
        /// </summary>
        /// <param name="subscriberKey">������Ψһ����ֵ</param>
        public static void UnRegist(string subscriberKey)
        {
            if (string.IsNullOrEmpty(subscriberKey))
            {
                return;
            }
            _subscribers.Remove(subscriberKey);
        }

        /// <summary>
        ///     ��ȡ����ָ��Ψһ����ֵ�Ķ�����
        /// </summary>
        /// <param name="subscriberKey">Ψһ����ֵ</param>
        /// <returns>���ض�����</returns>
        public static IConfigSubscriber GetSubscriber(string subscriberKey)
        {
            IConfigSubscriber subscriber;
            return _subscribers.TryGetValue(subscriberKey, out subscriber) ? subscriber : null;
        }

        /// <summary>
        ///     ��ȡ����ָ������ͨ����ŵĶ�����
        /// </summary>
        /// <param name="channelId">����ͨ�����</param>
        /// <returns>���ض�����</returns>
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