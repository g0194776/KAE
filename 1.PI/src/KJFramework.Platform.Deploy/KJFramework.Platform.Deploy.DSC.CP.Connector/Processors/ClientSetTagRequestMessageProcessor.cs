using System;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Client.ProtocolStack;

namespace KJFramework.Platform.Deploy.DSC.CP.Connector.Processors
{
    /// <summary>
    ///     客户端设置标示请求消息处理器
    /// </summary>
    public class ClientSetTagRequestMessageProcessor : FunctionProcessor<ClientMessage>
    {
        #region Overrides of FunctionProcessor<ClientMessage>

        public override ClientMessage Process(Guid id, ClientMessage message)
        {
            ClientSetTagRequestMessage msg = (ClientSetTagRequestMessage) message;
            Client.Add(msg.Header.ClientTag, id);
            ClientMessage responseMessage = new ClientSetTagResponseMessage {Result = true};
            responseMessage.Header.ClientTag = msg.Header.ClientTag;
            responseMessage.Header.TaskId = msg.Header.TaskId;
            return responseMessage;
        }

        #endregion
    }
}