using System;
using KJFramework.IO.Helper;
using KJFramework.Logger;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Client.ProtocolStack;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;

namespace KJFramework.Platform.Deploy.DSC.CP.Connector.Processors
{
    /// <summary>
    ///     客户端获取文件详细信息请求消息处理器
    /// </summary>
    public class ClientGetFileInfomationRequestMessageProcessor : FunctionProcessor<ClientMessage>
    {
        #region Overrides of FunctionProcessor<ClientMessage>

        public override ClientMessage Process(Guid id, ClientMessage message)
        {
            ClientGetFileInfomationRequestMessage msg = (ClientGetFileInfomationRequestMessage)message;
            try
            {
                DSCGetFileInfomationRequestMessage requestMessage = new DSCGetFileInfomationRequestMessage();
                requestMessage.Header.ClientTag = msg.Header.ClientTag;
                requestMessage.Header.TaskId = msg.Header.TaskId;
                requestMessage.ServiceName = msg.ServiceName;
                requestMessage.Files = msg.Files;
                requestMessage.MachineName = msg.MachineName;
                requestMessage.Bind();
                //boardcast.
                foreach (Guid channelId in DynamicServices.GetServiceOnMachine(msg.ServiceName, msg.MachineName))
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