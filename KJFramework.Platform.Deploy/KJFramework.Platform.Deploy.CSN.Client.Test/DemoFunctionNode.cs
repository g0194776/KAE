using KJFramework.Net.Cloud.Nodes;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.CSN.ProtocolStack;

namespace KJFramework.Platform.Deploy.CSN.Client.Test
{
    public class DemoFunctionNode : MessageFunctionNode<CSNMessage>
    {
        #region Overrides of FunctionNode<CSNMessage>

        public override bool Initialize()
        {
            return true;
        }

        #endregion

        #region Overrides of MessageFunctionNode<CSNMessage>

        protected override IFunctionProcessor<CSNMessage> CheckMessageCanProcessed(CSNMessage message)
        {
            switch (message.Header.ProtocolId)
            {
                //regist CSN response message.
                case 1:
                    return new CSNRegistResponseMessageProcessor();
                case 3:
                    return new CSNGetDataTableResponseMessageProcessor();
            }
            return null;
        }

        #endregion
    }
}