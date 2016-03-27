using System;
using KJFramework.IO.Helper;
using KJFramework.Logger;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Client.ProtocolStack;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;

namespace KJFramework.Platform.Deploy.DSC.CP.Connector.Processors
{
    /// <summary>
    ///     客户端更新组件请求消息处理器
    /// </summary>
    public class ClientUpdateComponentRequestMessageProcessor : FunctionProcessor<ClientMessage>
    {
        #region Overrides of FunctionProcessor<ClientMessage>

        public override ClientMessage Process(Guid id, ClientMessage message)
        {
            ClientUpdateComponentRequestMessage msg = (ClientUpdateComponentRequestMessage)message;
            try
            {
                DSCUpdateComponentRequestMessage requestMessage = new DSCUpdateComponentRequestMessage();
                requestMessage.Header.ClientTag = msg.Header.ClientTag;
                requestMessage.Header.TaskId = msg.Header.TaskId;
                requestMessage.ServiceName = msg.ServiceName;
                requestMessage.ComponentName = msg.ComponentName;
                requestMessage.FileName = msg.FileName;
                requestMessage.MachineName = msg.MachineName;
                requestMessage.Bind();
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