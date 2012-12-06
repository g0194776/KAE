using System;
using KJFramework.IO.Helper;
using KJFramework.Logger;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Client.ProtocolStack;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;

namespace KJFramework.Platform.Deploy.DSC.CP.Connector.Processors
{
    /// <summary>
    ///     客户端获取组件健康状态请求消息
    /// </summary>
    public class ClientGetComponentHealthRequestMessageProcessor : FunctionProcessor<ClientMessage>
    {
        #region Overrides of FunctionProcessor<ClientMessage>

        public override ClientMessage Process(Guid id, ClientMessage message)
        {
            ClientGetComponentHealthRequestMessage msg = (ClientGetComponentHealthRequestMessage) message;
            try
            {
                DSCGetComponentHealthRequestMessage requestMessage = new DSCGetComponentHealthRequestMessage();
                requestMessage.Header.ClientTag = msg.Header.ClientTag;
                requestMessage.Header.TaskId = msg.Header.TaskId;
                requestMessage.ServiceName = msg.ServiceName;
                requestMessage.Components = msg.Components;
                requestMessage.MachineName = msg.MachineName;
                requestMessage.Bind();
                //boardcast.
                foreach (Guid channelId in DynamicServices.GetServiceOnMachine(requestMessage.ServiceName, requestMessage.MachineName))
                    Global.CommnicationNode.Send(channelId, requestMessage.Body);   
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