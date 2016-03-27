using System;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.SMC.ProtocolStack.Services;

namespace KJFramework.Platform.Deploy.Maintenance.Processors
{
    /// <summary>
    ///     获取文件详细信息请求消息处理器
    /// </summary>
    public class DSGetFileInfomationRequestMessageProcessor : FunctionProcessor<DynamicServiceMessage>
    {
        #region Overrides of FunctionProcessor<DynamicServiceMessage>

        public override DynamicServiceMessage Process(Guid id, DynamicServiceMessage message)
        {
            DynamicServiceGetFileInfomationRequestMessage msg = (DynamicServiceGetFileInfomationRequestMessage) message;
            return Global.DynamicService.GetFileInfomation(msg);
        }

        #endregion
    }
}