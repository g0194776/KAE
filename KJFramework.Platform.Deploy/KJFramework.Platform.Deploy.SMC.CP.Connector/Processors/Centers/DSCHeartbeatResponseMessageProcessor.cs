using System;
using KJFramework.IO.Helper;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;

namespace KJFramework.Platform.Deploy.SMC.CP.Connector.Processors.Centers
{
    /// <summary>
    ///     DSC心跳回馈消息处理器
    /// </summary>
    public class DSCHeartbeatResponseMessageProcessor : FunctionProcessor<DSCMessage>
    {
        #region Overrides of FunctionProcessor<DSCMessage>

        public override DSCMessage Process(Guid id, DSCMessage message)
        {
            DSCHeartBeatResponseMessage responseMessage = (DSCHeartBeatResponseMessage)message;
            ConsoleHelper.PrintLine("#Heartbeat ack, Result : " + responseMessage.Result, ConsoleColor.DarkGray);
            return null;
        }

        #endregion
    }
}