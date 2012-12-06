using System;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;

namespace KJFramework.Platform.Deploy.CSN.CP.Basic.Processors
{
    /// <summary>
    ///     注册DSC回馈消息处理器
    /// </summary>
    public class DSCRegistResponseMessageProcessor : FunctionProcessor<DSCMessage>
    {
        #region Overrides of FunctionProcessor<DSCMessage>

        public override DSCMessage Process(Guid id, DSCMessage message)
        {
            DSCRegistResponseMessage responseMessage = (DSCRegistResponseMessage)message;
            if (Global.ConnectorInstance.RegistSignal != null)
            {
                Global.ConnectorInstance.RegistSignal.Set();
                Global.ConnectorInstance.RegistSignal = null;
                Global.ConnectorInstance.CallbackRegist(responseMessage.Result);
            }
            return null;
        }

        #endregion
    }
}