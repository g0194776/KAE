using System;
using KJFramework.IO.Helper;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.SMC.ProtocolStack.Services;

namespace KJFramework.Platform.Deploy.Maintenance.Processors
{
    /// <summary>
    ///     心跳回馈消息
    /// </summary>
    public class DSHeartBeatResponseMessageProcessor : FunctionProcessor<DynamicServiceMessage>
    {
        #region Overrides of FunctionProcessor<DynamicServiceMessage>

        public override DynamicServiceMessage Process(Guid id, DynamicServiceMessage message)
        {
            DynamicServiceHeartBeatResponseMessage msg = (DynamicServiceHeartBeatResponseMessage) message;
            ConsoleHelper.PrintLine("#Heartbeat ack, Result : " + msg.Result, ConsoleColor.DarkGray);
            return null;
        }

        #endregion
    }
}