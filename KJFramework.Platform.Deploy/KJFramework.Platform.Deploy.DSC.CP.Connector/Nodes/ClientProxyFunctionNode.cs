using KJFramework.Net.Cloud.Nodes;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Client.ProtocolStack;
using KJFramework.Platform.Deploy.DSC.CP.Connector.Processors;

namespace KJFramework.Platform.Deploy.DSC.CP.Connector.Nodes
{
    /// <summary>
    ///     客户代理功能节点
    /// </summary>
    public class ClientProxyFunctionNode : MessageFunctionNode<ClientMessage>
    {
        #region Overrides of FunctionNode<ClientMessage>

        public override bool Initialize()
        {
            return true;
        }

        #endregion

        #region Overrides of MessageFunctionNode<ClientMessage>

        protected override IFunctionProcessor<ClientMessage> CheckMessageCanProcessed(ClientMessage message)
        {
            switch (message.Header.ProtocolId)
            {
                case 0:
                    return new ClientSetTagRequestMessageProcessor();
                case 2:
                    return new ClientGetServicesRequestMessageProcessor();
                case 5:
                    return new ClientResetHeartBeatTimeRequestMessageProcessor();
                case 7:
                    return new ClientUpdateComponentRequestMessageProcessor();
                case 9:
                    return new ClientGetComponentHealthRequestMessageProcessor();
                case 12:
                    return new ClientGetFileInfomationRequestMessageProcessor();
                case 14:
                    return new ClientGetDeployNodesRequestMessageProcessor();
                case 16:
                    return new ClientGetCoreServiceRequestMessageProcessor();
            }
            return null;
        }

        #endregion
    }
}