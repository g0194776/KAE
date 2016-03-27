using System;
using KJFramework.IO.Helper;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;
using KJFramework.Platform.Deploy.SMC.Common.Performances;

namespace KJFramework.Platform.Deploy.DSC.CP.Connector.Processors
{
    /// <summary>
    ///     DSC心跳请求消息处理器
    /// </summary>
    public class DSCHeartBeatRequestMessageProcessor : FunctionProcessor<DSCMessage>
    {
        #region Overrides of FunctionProcessor<DSCMessage>

        public override DSCMessage Process(Guid id, DSCMessage message)
        {
            DSCHeartBeatRequestMessage msg = (DSCHeartBeatRequestMessage) message;
            ConsoleHelper.PrintLine("=>\r\nNew heartbeat request, details below: ", ConsoleColor.DarkGray);
            ConsoleHelper.PrintLine("Machine name: " + msg.MachineName, ConsoleColor.DarkGray);
            if (msg.PerformanceItems != null)
            {
                foreach (ServicePerformanceItem item in msg.PerformanceItems)
                {
                    ConsoleHelper.PrintLine(string.Format("{0}: {1}", item.PerformanceName, item.PerformanceValue), ConsoleColor.DarkGray);
                }
            }
            bool result = DynamicServices.Heartbeat(msg);
            ConsoleHelper.PrintLine("Result: " + result, ConsoleColor.DarkGray);
            return new DSCHeartBeatResponseMessage {Result = result};
        }

        #endregion
    }
}