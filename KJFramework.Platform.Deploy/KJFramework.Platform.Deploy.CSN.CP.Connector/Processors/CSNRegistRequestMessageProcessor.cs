using System;
using KJFramework.IO.Helper;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Enums;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Subscribers;
using KJFramework.Platform.Deploy.CSN.ProtocolStack;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector.Processors
{
    /// <summary>
    ///     CSN注册请求消息处理器
    /// </summary>
    public class CSNRegistRequestMessageProcessor : FunctionProcessor<CSNMessage>
    {
        #region Overrides of FunctionProcessor<CSNMessage>

        public override CSNMessage Process(Guid id, CSNMessage message)
        {
            CSNRegistRequestMessage msg = (CSNRegistRequestMessage) message;
            CSNRegistResponseMessage responseMessage = new CSNRegistResponseMessage();
            responseMessage.Header.ServiceKey = msg.Header.ServiceKey;
            IConfigSubscriber subscriber = ConfigSubscriberManager.Regist(SubscriberTypes.Database, msg, id);
            responseMessage.Result = subscriber != null;
            if (subscriber != null)
            {
                ConsoleHelper.PrintLine("#Config subscribed...  #key: " + subscriber.SubscriberKey + ", #update: " + subscriber.NeedUpdate, ConsoleColor.DarkGray);
            }
            return responseMessage;
        }

        #endregion
    }
}