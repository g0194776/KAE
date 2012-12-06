using System;
using KJFramework.IO.Helper;
using KJFramework.Logger;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Client.ProtocolStack;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;

namespace KJFramework.Platform.Deploy.DSC.CP.Connector.Processors
{
    /// <summary>
    ///     DSC更新状态通知请求消息处理器
    /// </summary>
    public class DSCUpdateProcessingMessageProcessor : FunctionProcessor<DSCMessage>
    {
        #region Overrides of FunctionProcessor<DSCMessage>

        public override DSCMessage Process(Guid id, DSCMessage message)
        {
            DSCUpdateProcessingMessage msg = (DSCUpdateProcessingMessage) message;
            DynamicServices.UpdateProcess(msg);
            #region Notify to client

            try
            {
                ClientUpdateProcessingMessage clientMessage = new ClientUpdateProcessingMessage();
                clientMessage.Header.ClientTag = msg.Header.ClientTag;
                clientMessage.Header.TaskId = msg.Header.TaskId;
                clientMessage.ComponentName = msg.ComponentName;
                clientMessage.Content = msg.Content;
                clientMessage.ServiceName = msg.ServiceName;
                clientMessage.Bind();
                Guid[] channelIds = Client.GetClient(clientMessage.Header.ClientTag);
                if (channelIds != null)
                {
                    foreach (Guid guid in channelIds)
                        Global.CommnicationNode.Send(guid, clientMessage.Body);
                }
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
                ConsoleHelper.PrintLine("Can not transport a object to component tunnel. Error message: \r\n" + ex.Message, ConsoleColor.DarkRed);
            }
            return null;

            #endregion
        }

        #endregion
    }
}