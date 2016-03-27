using System;
using KJFramework.IO.Helper;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;

namespace KJFramework.Platform.Deploy.DSN.CP.Connector.Processors
{
    /// <summary>
    ///     DSC����������Ϣ������
    /// </summary>
    public class DSCHeartbeatResponseMessageProcessor : FunctionProcessor<DSCMessage>
    {
        #region Overrides of FunctionProcessor<DSCMessage>

        public override DSCMessage Process(Guid id, DSCMessage message)
        {
            DSCHeartBeatResponseMessage responseMessage = (DSCHeartBeatResponseMessage)message;
            ConsoleHelper.PrintLine("#Heartbeat ack, Result : " + responseMessage.Result, ConsoleColor.DarkGray);
            return null;
        }

        #endregion
    }
}