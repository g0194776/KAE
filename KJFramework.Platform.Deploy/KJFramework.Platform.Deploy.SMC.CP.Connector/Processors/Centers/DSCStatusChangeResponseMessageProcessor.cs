using System;
using KJFramework.IO.Helper;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;

namespace KJFramework.Platform.Deploy.SMC.CP.Connector.Processors.Centers
{
    /// <summary>
    ///     DSC状态变更回馈消息处理器
    /// </summary>
    public class DSCStatusChangeResponseMessageProcessor : FunctionProcessor<DSCMessage>
    {
        #region Overrides of FunctionProcessor<DSCMessage>

        public override DSCMessage Process(Guid id, DSCMessage message)
        {
            DSCStatusChangeResponseMessage msg = (DSCStatusChangeResponseMessage) message;
            ConsoleHelper.PrintLine("Received a message of status changed response #Result = " + msg.Result, ConsoleColor.DarkGray);
            return null;
        }

        #endregion
    }
}