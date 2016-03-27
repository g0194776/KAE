using KJFramework.Net.Cloud.Nodes;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Processors;
using KJFramework.Platform.Deploy.CSN.ProtocolStack;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector.Nodes
{
    /// <summary>
    ///     配置分发的功能节点
    /// </summary>
    public class ConfigFunctionNode : MessageFunctionNode<CSNMessage>
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
                //CSN regist request message.
                case 0:
                    return new CSNRegistRequestMessageProcessor();
                //get data table config request message.
                case 2:
                    return new CSNGetDataTableRequestMessageProcessor();
            }
            return null;
        }

        #endregion
    }
}