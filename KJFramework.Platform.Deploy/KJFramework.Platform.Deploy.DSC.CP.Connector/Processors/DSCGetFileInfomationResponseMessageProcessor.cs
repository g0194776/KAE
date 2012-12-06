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
    ///     获取文件详细信息回馈消息处理器
    /// </summary>
    public class DSCGetFileInfomationResponseMessageProcessor : FunctionProcessor<DSCMessage>
    {
        #region Overrides of FunctionProcessor<DSCMessage>

        public override DSCMessage Process(Guid id, DSCMessage message)
        {
            DSCGetFileInfomationResponseMessage responseMessage = (DSCGetFileInfomationResponseMessage) message;
            ConsoleHelper.PrintLine("=>\r\nReceived a new get file infomations response message, details below: ", ConsoleColor.DarkGray);
            ConsoleHelper.PrintLine("Service name: " + responseMessage.ServiceName, ConsoleColor.DarkGray);
            if (responseMessage.Files != null)
            {
                foreach (FileInfo fileInfo in responseMessage.Files)
                {
                    ConsoleHelper.PrintLine("#File: " + fileInfo.FileName, ConsoleColor.DarkGray);
                    ConsoleHelper.PrintLine("    #IsExists: " + fileInfo.IsExists, ConsoleColor.DarkGray);
                    if (fileInfo.IsExists)
                    {
                        ConsoleHelper.PrintLine("    #Create time: " + fileInfo.CreateTime, ConsoleColor.DarkGray);
                        ConsoleHelper.PrintLine("    #Modify time: " + fileInfo.LastModifyTime, ConsoleColor.DarkGray);
                        ConsoleHelper.PrintLine("    #Size: " + fileInfo.Size, ConsoleColor.DarkGray);
                        ConsoleHelper.PrintLine("    #Path: " + fileInfo.Directory, ConsoleColor.DarkGray);
                    }
                }
            }
            //Notify to client.
            #region Notify to client

            try
            {
                ClientGetFileInfomationResponseMessage clientMessage = new ClientGetFileInfomationResponseMessage();
                clientMessage.Header.ClientTag = responseMessage.Header.ClientTag;
                clientMessage.Header.TaskId = responseMessage.Header.TaskId;
                clientMessage.ServiceName = responseMessage.ServiceName;
                clientMessage.Files = responseMessage.Files;
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