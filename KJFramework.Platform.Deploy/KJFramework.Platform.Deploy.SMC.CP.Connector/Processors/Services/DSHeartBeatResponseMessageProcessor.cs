using System;
using KJFramework.Basic.Enum;
using KJFramework.IO.Helper;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.Metadata.Performances;
using KJFramework.Platform.Deploy.SMC.Common.Performances;
using KJFramework.Platform.Deploy.SMC.ProtocolStack.Services;

namespace KJFramework.Platform.Deploy.SMC.CP.Connector.Processors.Services
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
            bool result = ServicePerformancer.Instance.HeartBeat(msg);
            ConsoleHelper.PrintLine(
                String.Format(
                    "=>\r\n#HeartBeat response :\r\nService Name: {0}\r\nComponent Count: {1}\r\nSupportDomainPerformance: {2}\r\nHeartBeat Result: {3}",
                    msg.ServiceName, msg.ComponentCount, msg.SupportDomainPerformance, result), ConsoleColor.DarkGray);
            if (msg.PerformanceItems != null)
            {
                foreach (ServicePerformanceItem item in msg.PerformanceItems)
                {
                    ConsoleHelper.PrintLine(String.Format("{0}: {1}", item.PerformanceName, item.PerformanceValue), ConsoleColor.DarkGray);
                }
                ConsoleHelper.PrintLine("Component details below: ", ConsoleColor.DarkGray);
                foreach (ComponentHealthItem item in msg.ComponentItems)
                {
                    ConsoleHelper.PrintLine("#Name: " + item.ComponentName + ", Health status: " + item.Status,
                                            (item.Status == HealthStatus.Death
                                                 ? ConsoleColor.DarkRed
                                                 : ConsoleColor.DarkGray));
                }
            }
            return null;
        }

        #endregion
    }
}