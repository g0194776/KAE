using System;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.SMC.ProtocolStack.Services;

namespace KJFramework.Platform.Deploy.SMC.CP.Connector.Processors.Services
{
    /// <summary>
    ///     重置心跳回复包处理器
    /// </summary>
    public class DSResetHeartBeatTimeResponseMessageProcessor : FunctionProcessor<DynamicServiceMessage>
    {
        #region Overrides of FunctionProcessor<DynamicServiceMessage>

        public override DynamicServiceMessage Process(Guid id, DynamicServiceMessage message)
        {
            DynamicServiceResetHeartBeatTimeResponseMessage responseMessage = (DynamicServiceResetHeartBeatTimeResponseMessage) message;
            Console.WriteLine("#Service : " + responseMessage.ServiceName + ", Reset heartbeat time result : " + responseMessage.Result);
            return null;
        }

        #endregion
    }
}