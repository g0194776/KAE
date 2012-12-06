using System;
using KJFramework.IO.Helper;
using KJFramework.Logger;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Client.ProtocolStack;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;

namespace KJFramework.Platform.Deploy.DSC.CP.Connector.Processors
{
    /// <summary>
    ///     DSC重置心跳时间的回馈消息处理器
    /// </summary>
    public class DSCResetHeartBeatTimeResponseMessageProcessor : FunctionProcessor<DSCMessage>
    {
        #region Overrides of FunctionProcessor<DSCMessage>

        public override DSCMessage Process(Guid id, DSCMessage message)
        {
            DSCResetHeartBeatTimeResponseMessage responseMessage = (DSCResetHeartBeatTimeResponseMessage) message;
            ConsoleHelper.PrintLine("=>\r\nReceived a reset heartbeat response message, Result = " + responseMessage.Result, ConsoleColor.DarkGray);
            #region Notify to client

            try
            {
                ClientResetHeartBeatTimeResponseMessage clientMessage = new ClientResetHeartBeatTimeResponseMessage();
                clientMessage.Header.ClientTag = responseMessage.Header.ClientTag;
                clientMessage.Header.TaskId = responseMessage.Header.TaskId;
                clientMessage.Result = responseMessage.Result;
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