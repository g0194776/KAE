using System;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;
using KJFramework.Platform.Deploy.SMC.ProtocolStack.Services;

namespace KJFramework.Platform.Deploy.SMC.CP.Connector.Processors.Services
{
    /// <summary>
    ///     获取文件详细信息回馈消息处理器
    /// </summary>
    public class DSGetFileInfomationResponseMessageProcessor : FunctionProcessor<DynamicServiceMessage>
    {
        #region Overrides of FunctionProcessor<DynamicServiceMessage>

        public override DynamicServiceMessage Process(Guid id, DynamicServiceMessage message)
        {
            DynamicServiceGetFileInfomationResponseMessage responseMessage = (DynamicServiceGetFileInfomationResponseMessage) message;
            //notice to center.
            DSCGetFileInfomationResponseMessage msg = new DSCGetFileInfomationResponseMessage();
            msg.Files = responseMessage.Files;
            msg.Header.ClientTag = responseMessage.Header.ClientTag;
            msg.ServiceName = responseMessage.ServiceName;
            msg.MachineName = Environment.MachineName;
            msg.Bind();
            Global.CenterNetworkNode.Send(Global.CenterId, msg.Body);
            return null;
        }

        #endregion
    }
}