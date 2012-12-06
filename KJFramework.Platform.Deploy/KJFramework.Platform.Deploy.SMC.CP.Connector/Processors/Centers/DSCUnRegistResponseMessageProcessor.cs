using System;
using KJFramework.IO.Helper;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;

namespace KJFramework.Platform.Deploy.SMC.CP.Connector.Processors.Centers
{
    /// <summary>
    ///     ע��DSC������Ϣ������
    /// </summary>
    public class DSCUnRegistResponseMessageProcessor : FunctionProcessor<DSCMessage>
    {
        #region Overrides of FunctionProcessor<DSCMessage>

        public override DSCMessage Process(Guid id, DSCMessage message)
        {
            DSCUnRegistResponseMessage msg = (DSCUnRegistResponseMessage)message;
            ConsoleHelper.PrintLine("Received a message of unregist response #Result = " + msg.Result, ConsoleColor.DarkGray);
            return null;
        }

        #endregion
    }
}