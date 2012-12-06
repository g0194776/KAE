using KJFramework.Net.Cloud.Nodes;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.CSN.CP.Basic.Processors;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;

namespace KJFramework.Platform.Deploy.CSN.CP.Basic.Nodes
{
    public class ConnectorFunctionNode : MessageFunctionNode<DSCMessage>
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
                //Heartbeat response message.
                case 5:
                    return new DSCHeartbeatResponseMessageProcessor();
                //Reset heartbeat time request message.
                case 10:
                    return new DSCResetHeartbeatRequestMessageProcessor();
            }
            return null;
        }

        #endregion
    }
}