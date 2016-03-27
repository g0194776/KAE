using System;
using KJFramework.IO.Helper;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.DSN.ProtocolStack;

namespace KJFramework.Platform.Deploy.DSN.Test.Client
{
    public class DSNEndDeployMessageProcessor : FunctionProcessor<DSNMessage>
    {
        #region Overrides of FunctionProcessor<DSNMessage>

        public override DSNMessage Process(Guid id, DSNMessage message)
        {
            DSNEndDeployMessage msg = (DSNEndDeployMessage) message;
            ConsoleHelper.PrintLine("=>\r\nReceived a new message of End Deploy Request Message, Details below: ", ConsoleColor.DarkGray);
            ConsoleHelper.PrintLine("#Request token: " + msg.RequestToken, ConsoleColor.DarkGray);
            ConsoleHelper.PrintLine("#IsDeployed: " + msg.IsDeployed, ConsoleColor.DarkGray);
            ConsoleHelper.PrintLine("#LastError: " + msg.LastError ?? "*None*", ConsoleColor.DarkGray);
            return null;
        }

        #endregion
    }
}