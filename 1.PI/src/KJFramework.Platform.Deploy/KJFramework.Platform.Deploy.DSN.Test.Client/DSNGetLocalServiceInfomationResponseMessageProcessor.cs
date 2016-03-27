using System;
using KJFramework.IO.Helper;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.DSN.ProtocolStack;
using KJFramework.Platform.Deploy.Metadata.Objects;

namespace KJFramework.Platform.Deploy.DSN.Test.Client
{
    public class DSNGetLocalServiceInfomationResponseMessageProcessor : FunctionProcessor<DSNMessage>
    {
        #region Overrides of FunctionProcessor<DSNMessage>

        public override DSNMessage Process(Guid id, DSNMessage message)
        {
            DSNGetLocalServiceInfomationResponseMessage responseMessage = (DSNGetLocalServiceInfomationResponseMessage) message;
            ConsoleHelper.PrintLine("=>\r\nReceive a new message of Get Local Service Infomation Response Message, details below: ", ConsoleColor.DarkGray);
            if (responseMessage.Services != null)
            {
                foreach (ServiceInfoItem item in responseMessage.Services)
                {
                    ConsoleHelper.PrintLine("#Service name: " + item.ServiceName);
                    ConsoleHelper.PrintLine("    #Name: " + item.Name ?? "*None*", ConsoleColor.DarkGray);
                    ConsoleHelper.PrintLine("    #Status: " + item.Status, ConsoleColor.DarkGray);
                }
            }
            return null;
        }

        #endregion
    }
}