using KJFramework.Net.Cloud.Nodes;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.DSC.CP.Connector.Processors;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;
using KJFramework.Platform.DSC.ProtocolStack;

namespace KJFramework.Platform.Deploy.DSC.CP.Connector.Nodes
{
    /// <summary>
    ///     DSC功能节点
    /// </summary>
    public class DSCFunctionNode : MessageFunctionNode<DSCMessage>
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
                //regist request message.
                case 0:
                    return new DSCRegistRequestMessageProcessor();
                //unregist request message.
                case 2:
                    return new DSCUnRegistRequestMessageProcessor();
                //heartbeat request message.
                case 4:
                    return new DSCHeartBeatRequestMessageProcessor();
                //get service details infomation response message.
                case 7:
                    return new DSCGetServicesResponseMessageProcessor();
                //status changed request message.
                case 8:
                    return new DSCStatusChangeRequestMessageProcessor();
                //reset heartbeat interval time response message.
                case 11:
                    return new DSCResetHeartBeatTimeResponseMessageProcessor();
                //component updated response message.
                case 13:
                    return new DSCUpdateComponentResponseMessageProcessor();
                //get component health status response message.
                case 15:
                    return new DSCGetComponentHealthResponseMessageProcessor();
                //component update processing message.
                case 16:
                    return new DSCUpdateProcessingMessageProcessor();
                //get file detail infomations response message.
                case 18:
                    return new DSCGetFileInfomationResponseMessageProcessor();
            }
            return null;
        }

        #endregion
    }
}