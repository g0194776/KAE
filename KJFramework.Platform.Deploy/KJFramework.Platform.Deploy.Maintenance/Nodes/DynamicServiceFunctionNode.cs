using KJFramework.Net.Cloud.Nodes;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.Maintenance.Processors;
using KJFramework.Platform.Deploy.SMC.ProtocolStack.Services;

namespace KJFramework.Platform.Deploy.Maintenance.Nodes
{
    /// <summary>
    ///     动态服务内部的功能节点
    /// </summary>
    internal class DynamicServiceFunctionNode : MessageFunctionNode<DynamicServiceMessage>
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
                //Regist response message.
                case 1:
                    return new DSRegistResponseMessageProcessor();
                //Heartbeat request message.
                case 2:
                    return new DSHeartBeatRequestMessageProcessor();
                //Reset heartbeat message.
                //case 4:
                //    return new DSResetHeartBeatTimeRequestMessageProcessor();
                //Component update message.
                case 6:
                    return new DSUpdateComponentRequestMessageProcessor();
                //Get Component health status message.
                case 9:
                    return new DSGetComponentHealthRequestMessageProcessor();
                //Get file detail infomations request message.
                case 11:
                    return new DSGetFileInfomationRequestMessageProcessor();
            }
            return null;
        }

        #endregion
    }
}