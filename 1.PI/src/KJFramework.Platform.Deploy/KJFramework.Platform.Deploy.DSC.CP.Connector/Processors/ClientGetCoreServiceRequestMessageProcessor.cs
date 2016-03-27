using System;
using KJFramework.IO.Helper;
using KJFramework.Logger;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Client.ProtocolStack;

namespace KJFramework.Platform.Deploy.DSC.CP.Connector.Processors
{
    /// <summary>
    ///     客户端获取核心服务信息请求消息处理器
    /// </summary>
    public class ClientGetCoreServiceRequestMessageProcessor : FunctionProcessor<ClientMessage>
    {
        #region Overrides of FunctionProcessor<ClientMessage>

        public override ClientMessage Process(Guid id, ClientMessage message)
        {
            try
            {
                ClientGetCoreServiceRequestMessage msg = (ClientGetCoreServiceRequestMessage) message;
                ClientGetCoreServiceResponseMessage rspMessage = new ClientGetCoreServiceResponseMessage();
                rspMessage.Header.ClientTag = message.Header.ClientTag;
                rspMessage.Header.TaskId = msg.Header.TaskId;
                rspMessage.Items = DynamicServices.GetCoreService(msg.Category);
                return rspMessage;
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
                ConsoleHelper.PrintLine("Can not transport a object to component tunnel. Error message: \r\n" + ex.Message, ConsoleColor.DarkRed);
            }
            return null;
        }

        #endregion
    }
}