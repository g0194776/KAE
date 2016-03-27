using System;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.SMC.ProtocolStack.Services;

namespace KJFramework.Platform.Deploy.Maintenance.Processors
{
    /// <summary>
    ///     获取组件健康状态请求处理器
    /// </summary>
    public class DSGetComponentHealthRequestMessageProcessor : FunctionProcessor<DynamicServiceMessage>
    {
        #region Overrides of FunctionProcessor<DynamicServiceMessage>

        public override DynamicServiceMessage Process(Guid id, DynamicServiceMessage message)
        {
            DynamicServiceGetComponentHealthRequestMessage msg = (DynamicServiceGetComponentHealthRequestMessage) message;
            DynamicServiceGetComponentHealthResponseMessage  responseMessage = new DynamicServiceGetComponentHealthResponseMessage();
            responseMessage.ServiceName = Global.DynamicService.Infomation.ServiceName;
            responseMessage.Items = Global.DynamicService.GetComponentHealth(msg.Components);
            responseMessage.Header.ClientTag = msg.Header.ClientTag;
            responseMessage.Header.TaskId = msg.Header.TaskId;
            return responseMessage;
        }

        #endregion
    }
}