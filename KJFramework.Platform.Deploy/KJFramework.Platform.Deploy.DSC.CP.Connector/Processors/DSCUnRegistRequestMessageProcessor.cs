using System;
using KJFramework.IO.Helper;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;

namespace KJFramework.Platform.Deploy.DSC.CP.Connector.Processors
{
    /// <summary>
    ///     DSC注销请求消息处理器
    /// </summary>
    public class DSCUnRegistRequestMessageProcessor : FunctionProcessor<DSCMessage>
    {
        #region Overrides of FunctionProcessor<DSCMessage>

        public override DSCMessage Process(Guid id, DSCMessage message)
        {
            DSCUnRegistRequestMessage msg = (DSCUnRegistRequestMessage) message;
            ConsoleHelper.PrintLine("=>\r\nReceived a unregist request message, details below:", ConsoleColor.DarkYellow);
            ConsoleHelper.PrintLine("Machine name: " + msg.MachineName, ConsoleColor.DarkYellow);
            ConsoleHelper.PrintLine("Reason: " + (msg.Reason ?? "*None*"), ConsoleColor.DarkYellow);
            DynamicServices.UnRegist(msg);
            return null;
        }

        #endregion
    }
}