using System;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;

namespace KJFramework.Platform.Deploy.DSN.CP.Connector.Processors
{
    /// <summary>
    ///     DSC重置心跳时间请求消息处理器
    /// </summary>
    public class DSCResetHeartbeatRequestMessageProcessor : FunctionProcessor<DSCMessage>
    {
        #region Overrides of FunctionProcessor<DSCMessage>

        public override DSCMessage Process(Guid id, DSCMessage message)
        {
            DSCResetHeartBeatTimeRequestMessage msg = (DSCResetHeartBeatTimeRequestMessage) message;
            Global.ConnectorInstance.ResetHeartbeatTime(msg.Interval);
            DSCMessage responseMessage =  new DSCResetHeartBeatTimeResponseMessage { Result = true, MachineName = Global.MachineName};
            responseMessage.Header.ClientTag = msg.Header.ClientTag;
            return responseMessage;
        }

        #endregion
    }
}