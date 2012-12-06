using KJFramework.Net.Cloud.Nodes;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.DSN.CP.Deployer.Processors;
using KJFramework.Platform.Deploy.DSN.ProtocolStack;

namespace KJFramework.Platform.Deploy.DSN.CP.Deployer.Nodes
{
    public class DeployerFunctionNode : MessageFunctionNode<DSNMessage>
    {
        #region Overrides of FunctionNode<DSNMessage>

        public override bool Initialize()
        {
            return true;
        }

        #endregion

        #region Overrides of MessageFunctionNode<DSNMessage>

        protected override IFunctionProcessor<DSNMessage> CheckMessageCanProcessed(DSNMessage message)
        {
            switch (message.Header.ProtocolId)
            {
                //Begin transfer file request message.
                case 0:
                    return new DSNBeginTransferFileRequestMessageProcessor();
                //Transfer data message.
                case 2:
                    return new DSNTransferDataMessageProcessor();
                //End transfer file request message.
                case 3:
                    return new DSNEndTransferFileRequestMessageProcessor();
                //Loss compensation message.
                case 5:
                    return new DSNLossCompensationMessageProcessor();
                //Begin deploy request message.
                case 6:
                    return new DSNBeginDeployMessageProcessor();
                //Un install service request message.
                case 9:
                    return new DSNUnDeployServiceRequestMessageProcessor();
                //Start service request message.
                case 11:
                    return new DSNStartServiceRequestMessageProcessor();
                //Stop service request message.
                case 13:
                    return new DSNStopServiceRequestMessageProcessor();
                //Get local service detail infomation request message.
                case 15:
                    return new DSNGetLocalServiceInfomationRequestMessageProcessor();
            }
            return null;
        }

        #endregion
    }
}