using System;
using KJFramework.IO.Helper;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;
using KJFramework.Platform.Deploy.Metadata.Objects;
using KJFramework.Platform.Deploy.SMC.ProtocolStack.Services;

namespace KJFramework.Platform.Deploy.SMC.CP.Connector.Processors.Services
{
    /// <summary>
    ///     更新组件回馈消息处理器
    /// </summary>
    public class DSUpdateComponentResponseMessageProcessor : FunctionProcessor<DynamicServiceMessage>
    {
        #region Overrides of FunctionProcessor<DynamicServiceMessage>

        public override DynamicServiceMessage Process(Guid id, DynamicServiceMessage message)
        {
            DynamicServiceUpdateComponentResponseMessage responseMessage = (DynamicServiceUpdateComponentResponseMessage) message;
            ConsoleHelper.PrintLine(string.Format("=>\r\nReceived component update response:\r\nService Name: {0}\r\nTotal update result: {1}\r\n", responseMessage.ServiceName, responseMessage.Result));
            if (responseMessage.Items != null)
            {
                ConsoleHelper.PrintLine("Details: ");
                foreach (ComponentUpdateResultItem item in responseMessage.Items)
                {
                    ConsoleHelper.Print(string.Format("#Component: {0}, Update result: {1}\r\n", item.ComponentName, item.Result));
                    if (!item.Result)
                    {
                        ConsoleHelper.PrintLine("Error trace : " + item.ErrorTrace, ConsoleColor.DarkRed);
                        continue;
                    }
                    Console.WriteLine();
                }
            }
            //notify to center.
            DSCUpdateComponentResponseMessage  dscUpdateComponentResponseMessage = new DSCUpdateComponentResponseMessage();
            dscUpdateComponentResponseMessage.Header.ClientTag = responseMessage.Header.ClientTag;
            dscUpdateComponentResponseMessage.ErrorTrace = responseMessage.ErrorTrace;
            dscUpdateComponentResponseMessage.MachineName = Environment.MachineName;
            dscUpdateComponentResponseMessage.Items = responseMessage.Items;
            dscUpdateComponentResponseMessage.Result = responseMessage.Result;
            dscUpdateComponentResponseMessage.ServiceName = responseMessage.ServiceName;
            dscUpdateComponentResponseMessage.Bind();
            Global.CenterNetworkNode.Send(Global.CenterId, dscUpdateComponentResponseMessage.Body);
            ServicePerformancer.Instance.Update(responseMessage);
            return null;
        }

        #endregion
    }
}