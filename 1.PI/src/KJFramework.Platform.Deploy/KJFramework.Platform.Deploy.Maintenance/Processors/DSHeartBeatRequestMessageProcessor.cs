using System;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.SMC.ProtocolStack.Services;

namespace KJFramework.Platform.Deploy.Maintenance.Processors
{
    /// <summary>
    ///     心跳请求消息处理器
    /// </summary>
    public class DSHeartBeatRequestMessageProcessor : FunctionProcessor<DynamicServiceMessage>
    {
        #region Overrides of FunctionProcessor<DynamicServiceMessage>

        public override DynamicServiceMessage Process(Guid id, DynamicServiceMessage message)
        {
            return Global.DynamicService.Heartbeat(message.Header.ClientTag);
            //return null;
        }

        #endregion
    }
}