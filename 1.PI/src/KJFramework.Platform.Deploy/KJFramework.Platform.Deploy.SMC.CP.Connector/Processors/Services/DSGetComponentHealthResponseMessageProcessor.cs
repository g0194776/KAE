using System;
using KJFramework.IO.Helper;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;
using KJFramework.Platform.Deploy.Metadata.Performances;
using KJFramework.Platform.Deploy.SMC.ProtocolStack.Services;

namespace KJFramework.Platform.Deploy.SMC.CP.Connector.Processors.Services
{
    /// <summary>
    ///     获取组件健康状态回馈消息处理器
    /// </summary>
    public class DSGetComponentHealthResponseMessageProcessor : FunctionProcessor<DynamicServiceMessage>
    {
        #region Overrides of FunctionProcessor<DynamicServiceMessage>

        public override DynamicServiceMessage Process(Guid id, DynamicServiceMessage message)
        {
            DynamicServiceGetComponentHealthResponseMessage responseMessage = (DynamicServiceGetComponentHealthResponseMessage) message;
            ConsoleHelper.PrintLine(
                string.Format(
                    "=>\r\nReceived get component health response message, details below: \r\nServiceName: {0}",
                    responseMessage.ServiceName), ConsoleColor.DarkGray);
            if (responseMessage.Items != null)
            {
                foreach (ComponentHealthItem item in responseMessage.Items)
                {
                    ConsoleHelper.PrintLine(string.Format("Component name: {0}, Health status: {1}", item.ComponentName, item.Status), ConsoleColor.DarkGray);
                }
            }
            //notify to center.
            DSCGetComponentHealthResponseMessage dscGetComponentHealthResponseMessage = new DSCGetComponentHealthResponseMessage();
            dscGetComponentHealthResponseMessage.Header.ClientTag = responseMessage.Header.ClientTag;
            dscGetComponentHealthResponseMessage.ServiceName = responseMessage.ServiceName;
            dscGetComponentHealthResponseMessage.Items = responseMessage.Items;
            dscGetComponentHealthResponseMessage.MachineName = Environment.MachineName;
            dscGetComponentHealthResponseMessage.Bind();
            Global.CenterNetworkNode.Send(Global.CenterId, dscGetComponentHealthResponseMessage.Body);
            ServicePerformancer.Instance.Update(responseMessage);
            return null;
        }

        #endregion
    }
}