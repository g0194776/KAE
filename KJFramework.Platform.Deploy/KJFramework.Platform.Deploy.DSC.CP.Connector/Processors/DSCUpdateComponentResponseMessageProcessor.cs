using System;
using KJFramework.IO.Helper;
using KJFramework.Logger;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Client.ProtocolStack;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;
using KJFramework.Platform.Deploy.Metadata.Objects;

namespace KJFramework.Platform.Deploy.DSC.CP.Connector.Processors
{
    /// <summary>
    ///     DSC组件更新回馈消息处理器
    /// </summary>
    public class DSCUpdateComponentResponseMessageProcessor : FunctionProcessor<DSCMessage>
    {
        #region Overrides of FunctionProcessor<DSCMessage>

        public override DSCMessage Process(Guid id, DSCMessage message)
        {
            DSCUpdateComponentResponseMessage responseMessage = (DSCUpdateComponentResponseMessage) message;
            ConsoleHelper.PrintLine(string.Format("=>\r\nReceived component update response:\r\nService Name: {0}\r\nTotal update result: {1}", responseMessage.ServiceName, responseMessage.Result));
            ConsoleHelper.PrintLine("Machine name: " + responseMessage.MachineName);
            if (responseMessage.Items != null)
            {
                ConsoleHelper.PrintLine("Details: ");
                foreach (ComponentUpdateResultItem item in responseMessage.Items)
                {
                    ConsoleHelper.Print(string.Format("#Component: {0}, Update result: {1}\r\n", item.ComponentName, item.Result));
                    if (!item.Result)
                    {
                        ConsoleHelper.PrintLine("Error trace : " + item.ErrorTrace, ConsoleColor.DarkRed);
                        continue;
                    }
                    Console.WriteLine();
                }
            }            
            #region Notify to client

            try
            {
                ClientUpdateComponentResponseMessage clientMessage = new ClientUpdateComponentResponseMessage();
                clientMessage.Header.ClientTag = responseMessage.Header.ClientTag;
                clientMessage.Header.TaskId = responseMessage.Header.TaskId;
                clientMessage.Result = responseMessage.Result;
                clientMessage.ErrorTrace = responseMessage.ErrorTrace;
                clientMessage.Items = responseMessage.Items;
                clientMessage.ServiceName = responseMessage.ServiceName;
                clientMessage.MachineName = responseMessage.MachineName;
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