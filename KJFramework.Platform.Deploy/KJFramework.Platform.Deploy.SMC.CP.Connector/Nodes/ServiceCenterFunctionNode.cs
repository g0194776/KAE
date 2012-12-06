using KJFramework.Net.Cloud.Nodes;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;
using KJFramework.Platform.Deploy.SMC.CP.Connector.Processors.Centers;
using KJFramework.Platform.DSC.ProtocolStack;

namespace KJFramework.Platform.Deploy.SMC.CP.Connector.Nodes
{
    /// <summary>
    ///     服务中心功能节点
    /// </summary>
    public class ServiceCenterFunctionNode : MessageFunctionNode<DSCMessage>
    {
        #region Overrides of FunctionNode<DSCMessage>

        public override bool Initialize()
        {
            return true;
        }

        #endregion

        #region Overrides of MessageFunctionNode<DSCMessage>

        protected override IFunctionProcessor<DSCMessage> CheckMessageCanProcessed(DSCMessage message)
        {
            switch (message.Header.ProtocolId)
            {
                //Regist response message.
                case 1:
                    return new DSCRegistResponseMessageProcessor();
                //UnRegist response message.
                case 3:
                    return new DSCUnRegistResponseMessageProcessor();
                //Heartbeat response message.
                case 5:
                    return new DSCHeartbeatResponseMessageProcessor();
                //Get services request message.
                case 6:
                    return new DSCGetServicesRequestMessageProcessor();
                //Status change reponse message.
                case 9:
                    return new DSCStatusChangeResponseMessageProcessor();
                //Reset heartbeat time request message.
                case 10:
                    return new DSCResetHeartbeatRequestMessageProcessor();
                //Update component request message.
                case 12:
                    return new DSCUpdateComponentRequestMessageProcessor();
                //Get component health status request message.
                case 14:
                    return new DSCGetComponentHealthRequestMessageProcessor();
                //get file detail infomations request message.
                case 17:
                    return new DSCGetFileInfomationRequestMessageProcessor();
            }
            return null;
        }

        #endregion
    }
}