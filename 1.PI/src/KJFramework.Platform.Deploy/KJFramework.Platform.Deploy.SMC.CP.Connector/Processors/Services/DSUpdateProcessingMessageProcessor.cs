using System;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;
using KJFramework.Platform.Deploy.SMC.ProtocolStack.Services;

namespace KJFramework.Platform.Deploy.SMC.CP.Connector.Processors.Services
{
    /// <summary>
    ///     组件更新状态通知消息处理器
    /// </summary>
    public class DSUpdateProcessingMessageProcessor : FunctionProcessor<DynamicServiceMessage>
    {
        #region Overrides of FunctionProcessor<DynamicServiceMessage>

        public override DynamicServiceMessage Process(Guid id, DynamicServiceMessage message)
        {
            DynamicServiceUpdateProcessingMessage msg = (DynamicServiceUpdateProcessingMessage) message;
            ServicePerformancer.Instance.NotifyUpdateProcessing(msg);

            //Notify to center service.
            DSCUpdateProcessingMessage requestMessage = new DSCUpdateProcessingMessage();
            requestMessage.Header.ClientTag = msg.Header.ClientTag;
            requestMessage.ComponentName = msg.ComponentName;
            requestMessage.ServiceName = msg.ServiceName;
            requestMessage.Content = msg.Content;
            requestMessage.Bind();
            if (Global.CenterNetworkNode != null)
            {
                Global.CenterNetworkNode.Send(Global.CenterId, requestMessage.Body);
            }
            return null;
        }

        #endregion
    }
}