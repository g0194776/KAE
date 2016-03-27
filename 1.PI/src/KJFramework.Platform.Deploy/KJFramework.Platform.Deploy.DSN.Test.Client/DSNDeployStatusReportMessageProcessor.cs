using System;
using KJFramework.IO.Helper;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.DSN.ProtocolStack;

namespace KJFramework.Platform.Deploy.DSN.Test.Client
{
    public class DSNDeployStatusReportMessageProcessor : FunctionProcessor<DSNMessage>
    {
        #region Overrides of FunctionProcessor<DSNMessage>

        public override DSNMessage Process(Guid id, DSNMessage message)
        {
            DSNDeployStatusReportMessage msg = (DSNDeployStatusReportMessage) message;
            ConsoleHelper.PrintLine("[Notify] " + msg.CurrentStatus, ConsoleColor.DarkGray);
            return null;
        }

        #endregion
    }
}