using System;
using KJFramework.Logger;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Enums;
using KJFramework.Platform.Deploy.CSN.ProtocolStack;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector.Subscribers
{
    /// <summary>
    ///     �����߹���
    /// </summary>
    public static class SubscriberFactory
    {
        #region Methods

        /// <summary>
        ///     ����һ��������
        /// </summary>
        /// <param name="subscriberTypes">����������</param>
        /// <param name="requestMessage">������ע��CSN��������Ϣ</param>
        /// <param name="channelId">�����ߵ�����ͨ�����</param>
        /// <returns>���ش�����Ķ�����</returns>
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