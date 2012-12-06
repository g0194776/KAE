using KJFramework.Net.Cloud.Nodes;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.SMC.CP.Connector.Processors;
using KJFramework.Platform.Deploy.SMC.CP.Connector.Processors.Services;
using KJFramework.Platform.Deploy.SMC.ProtocolStack.Services;

namespace KJFramework.Platform.Deploy.SMC.CP.Connector.Nodes
{
    /// <summary>
    ///     动态服务功能节点
    /// </summary>
    public class DynamicServiceFunctionNode : MessageFunctionNode<DynamicServiceMessage>
    {
        #region Overrides of FunctionNode<DynamicServiceMessage>

        public override bool Initialize()
        {
            return true;
        }

        #endregion

        #region Overrides of MessageFunctionNode<DynamicServiceMessage>

        protected override IFunctionProcessor<DynamicServiceMessage> CheckMessageCanProcessed(DynamicServiceMessage message)
        {
            switch (message.Header.ProtocolId)
            {
                //Regist request.
                case 0:
                    return new DSRegistRequestMessageProcessor();
                //HeartBeat response.
                case 3:
                    return new DSHeartBeatResponseMessageProcessor();
                //Reset heartbeat time response.
                case 5:
                    return new DSResetHeartBeatTimeResponseMessageProcessor();
                //Update component response.
                case 7:
                    return new DSUpdateComponentResponseMessageProcessor();
                //Update processing message.
                case 8:
                    return new DSUpdateProcessingMessageProcessor();
                //Get component health response message.
                case 10:
                    return new DSGetComponentHealthResponseMessageProcessor();
                //Get file detail infomation response message.
                case 12:
                    return new DSGetFileInfomationResponseMessageProcessor();
            }
            return null;
        }

        #endregion
    }
}