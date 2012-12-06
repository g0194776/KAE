using System;
using KJFramework.IO.Helper;
using KJFramework.Logger;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Client.ProtocolStack;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;
using KJFramework.Platform.Deploy.Metadata.Performances;

namespace KJFramework.Platform.Deploy.DSC.CP.Connector.Processors
{
    /// <summary>
    ///     DSC获取组件健康状态回馈消息处理器
    /// </summary>
    public class DSCGetComponentHealthResponseMessageProcessor : FunctionProcessor<DSCMessage>
    {
        #region Overrides of FunctionProcessor<DSCMessage>

        public override DSCMessage Process(Guid id, DSCMessage message)
        {
            DSCGetComponentHealthResponseMessage responseMessage = (DSCGetComponentHealthResponseMessage) message;
            ConsoleHelper.PrintLine("=>\r\nReceived a message of get component health response, details below: ", ConsoleColor.DarkGray);
            ConsoleHelper.PrintLine("Service Name: " + responseMessage.ServiceName, ConsoleColor.DarkGray);
            if (responseMessage.LastError != null)
            {
                ConsoleHelper.PrintLine("Last Error: " + responseMessage.LastError, ConsoleColor.DarkGray);
            }
            if (responseMessage.Items != null)
            {
                ConsoleHelper.PrintLine("Components infomation, details below: ", ConsoleColor.DarkGray);
                foreach (ComponentHealthItem item in responseMessage.Items)
                {
                    ConsoleHelper.PrintLine("#Component Name: " + item.ComponentName, ConsoleColor.DarkGray);
                    ConsoleHelper.PrintLine("#Component Status: " + item.Status, ConsoleColor.DarkGray);
                }
            }
            DynamicServices.Update(responseMessage);
            //Notify to client.
            #region Notify to client

            try
            {
                ClientGetComponentHealthResponseMessage clientMessage = new ClientGetComponentHealthResponseMessage();
                clientMessage.Header.ClientTag = responseMessage.Header.ClientTag;
                clientMessage.Header.TaskId = responseMessage.Header.TaskId;
                clientMessage.ServiceName = responseMessage.ServiceName;
                clientMessage.Items = responseMessage.Items;
                clientMessage.MachineName = responseMessage.MachineName;
                clientMessage.LastError = responseMessage.LastError;
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