using System;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.SMC.ProtocolStack.Services;

namespace KJFramework.Platform.Deploy.Maintenance.Processors
{
    /// <summary>
    ///     更新组件请求消息处理器
    /// </summary>
    public class DSUpdateComponentRequestMessageProcessor : FunctionProcessor<DynamicServiceMessage>
    {
        #region Overrides of FunctionProcessor<DynamicServiceMessage>

        public override DynamicServiceMessage Process(Guid id, DynamicServiceMessage message)
        {
            DynamicServiceUpdateComponentRequestMessage msg = (DynamicServiceUpdateComponentRequestMessage) message;
            DynamicServiceUpdateComponentResponseMessage responseMessage = new DynamicServiceUpdateComponentResponseMessage();
            responseMessage.Header.ClientTag = msg.Header.ClientTag;
            responseMessage.Header.TaskId = msg.Header.TaskId;
            bool result;
            responseMessage.Items = Global.DynamicService.Update(msg.ComponentName, msg.FileName, msg.Header.ClientTag, msg.Header.TaskId, out result);
            responseMessage.ServiceName = Global.DynamicService.Infomation.ServiceName;
            responseMessage.Result = result;
            return responseMessage;
        }

        #endregion
    }
}