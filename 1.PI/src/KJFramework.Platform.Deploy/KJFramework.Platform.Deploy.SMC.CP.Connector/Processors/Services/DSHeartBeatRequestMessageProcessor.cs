using System;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.SMC.ProtocolStack.Services;

namespace KJFramework.Platform.Deploy.SMC.CP.Connector.Processors.Services
{
    /// <summary>
    ///     心跳请求消息处理器
    ///     <para>* 此消息处理器已经废除。</para>
    /// </summary>
    public class DSHeartBeatRequestMessageProcessor : FunctionProcessor<DynamicServiceMessage>
    {
        #region Overrides of FunctionProcessor<DynamicServiceMessage>

        public override DynamicServiceMessage Process(Guid id, DynamicServiceMessage message)
        {
            //DynamicServiceHeartBeatRequestMessage msg = (DynamicServiceHeartBeatRequestMessage) message;
            //bool result = ServicePerformancer.Instance.HeartBeat(msg);
            //ConsoleHelper.PrintLine(
            //    String.Format(
            //        "=>\r\n#HeartBeat request :\r\nService Name: {0}\r\nComponent Count: {1}\r\nSupportDomainPerformance: {2}\r\nHeartBeat Result: {3}",
            //        msg.ServiceName, msg.ComponentCount, msg.SupportDomainPerformance, result), ConsoleColor.DarkGray);
            //if (msg.PerformanceItems != null)
            //{
            //    foreach (ServicePerformanceItem item in msg.PerformanceItems)
            //    {
            //        ConsoleHelper.PrintLine(String.Format("{0}: {1}", item.PerformanceName, item.PerformanceValue), ConsoleColor.DarkGray);
            //    }
            //    ConsoleHelper.PrintLine("Component details below: ", ConsoleColor.DarkGray);
            //    foreach (ComponentHealthItem item in msg.ComponentItems)
            //    {
            //        ConsoleHelper.PrintLine("#Name: " + item.ComponentName + ", Health status: " + item.Status,
            //                                (item.Status == HealthStatus.Death
            //                                     ? ConsoleColor.DarkRed
            //                                     : ConsoleColor.DarkGray));
            //    }
            //}
            //DynamicServiceHeartBeatResponseMessage responseMessage = new DynamicServiceHeartBeatResponseMessage();
            //responseMessage.Result = result;
            //return responseMessage;
            return null;
        }

        #endregion
    }
}