using System;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;

namespace KJFramework.Platform.Deploy.SMC.CP.Connector.Processors.Centers
{
    /// <summary>
    ///     ע��DSC������Ϣ������
    /// </summary>
    public class DSCRegistResponseMessageProcessor : FunctionProcessor<DSCMessage>
    {
        #region Overrides of FunctionProcessor<DSCMessage>

        public override DSCMessage Process(Guid id, DSCMessage message)
        {
            DSCRegistResponseMessage responseMessage = (DSCRegistResponseMessage)message;
            Global.ConnectorInstance.RegistSignal.Set();
            Global.ConnectorInstance.RegistSignal = null;
            Global.ConnectorInstance.CallbackRegist(responseMessage.Result);
            return null;
        }

        #endregion
    }
}