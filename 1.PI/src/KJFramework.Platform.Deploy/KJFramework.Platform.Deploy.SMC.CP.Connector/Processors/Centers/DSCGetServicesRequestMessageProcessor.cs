using System;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;

namespace KJFramework.Platform.Deploy.SMC.CP.Connector.Processors.Centers
{
    /// <summary>
    ///     DSC��ȡ��������������Ϣ������
    /// </summary>
    public class DSCGetServicesRequestMessageProcessor : FunctionProcessor<DSCMessage>
    {
        #region Overrides of FunctionProcessor<DSCMessage>

        public override DSCMessage Process(Guid id, DSCMessage message)
        {
            DSCGetServicesRequestMessage msg = (DSCGetServicesRequestMessage) message;
            DSCGetServicesResponseMessage responseMessage = ServicePerformancer.Instance.GetServiceInfomation(msg);
            responseMessage.Header.ClientTag = msg.Header.ClientTag;
            return responseMessage;
        }

        #endregion
    }
}