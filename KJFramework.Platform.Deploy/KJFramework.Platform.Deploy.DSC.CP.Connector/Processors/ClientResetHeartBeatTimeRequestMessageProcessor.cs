using System;
using KJFramework.IO.Helper;
using KJFramework.Logger;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Client.ProtocolStack;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;

namespace KJFramework.Platform.Deploy.DSC.CP.Connector.Processors
{
    /// <summary>
    ///     客户端重置心跳时间间隔请求消息处理器
    /// </summary>
    public class ClientResetHeartBeatTimeRequestMessageProcessor : FunctionProcessor<ClientMessage>
    {
        #region Overrides of FunctionProcessor<ClientMessage>

        public override ClientMessage Process(Guid id, ClientMessage message)
        {
            ClientResetHeartBeatTimeRequestMessage msg = (ClientResetHeartBeatTimeRequestMessage)message;
            try
            {
                DSCResetHeartBeatTimeRequestMessage requestMessage = new DSCResetHeartBeatTimeRequestMessage();
                requestMessage.Header.ClientTag = msg.Header.ClientTag;
                requestMessage.Header.TaskId = msg.Header.TaskId;
                requestMessage.MachineName = msg.MachineName;
                requestMessage.Target = msg.Target;
                requestMessage.Interval = msg.Interval;
                requestMessage.Bind();
                foreach (Guid channelId in DynamicServices.GetServiceOnMachine(requestMessage.MachineName, requestMessage.Target))
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