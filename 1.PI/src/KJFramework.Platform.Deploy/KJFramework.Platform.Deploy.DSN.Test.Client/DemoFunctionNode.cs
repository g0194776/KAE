using KJFramework.Net.Cloud.Nodes;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.DSN.ProtocolStack;

namespace KJFramework.Platform.Deploy.DSN.Test.Client
{
    public class DemoFunctionNode : MessageFunctionNode<DSNMessage>
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
                case 4:
                    return new DSNEndTransferFileResponseMessageProcessor();
                case 7:
                    return new DSNEndDeployMessageProcessor();
                case 8:
                    return new DSNDeployStatusReportMessageProcessor();
                case 16:
                    return new DSNGetLocalServiceInfomationResponseMessageProcessor();
            }
            return null;
        }

        #endregion
    }
}