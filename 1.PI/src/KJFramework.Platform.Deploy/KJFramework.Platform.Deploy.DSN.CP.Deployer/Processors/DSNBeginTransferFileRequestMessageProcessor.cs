using System;
using KJFramework.IO.Helper;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.DSN.ProtocolStack;

namespace KJFramework.Platform.Deploy.DSN.CP.Deployer.Processors
{
    /// <summary>
    ///     开始传输文件请求消息处理器
    /// </summary>
    public class DSNBeginTransferFileRequestMessageProcessor : FunctionProcessor<DSNMessage>
    {
        #region Overrides of FunctionProcessor<DSNMessage>

        public override DSNMessage Process(Guid id, DSNMessage message)
        {
            DSNBeginTransferFileRequestMessage requestMessage = (DSNBeginTransferFileRequestMessage) message;
            string reason;
            bool result = DataBus.AddPackage(requestMessage, out reason);
            DSNBeginTransferFileResponseMessage responseMessage = new DSNBeginTransferFileResponseMessage();
            responseMessage.RequestToken = requestMessage.RequestToken;
            responseMessage.Reason = reason;
            responseMessage.IsAccept = result;
            ConsoleHelper.PrintLine("=>\r\nReceived a new message of Begin Transfer File Request Message, Details below: ", ConsoleColor.DarkGray);
            ConsoleHelper.PrintLine("#Request token: " + requestMessage.RequestToken, ConsoleColor.DarkGray);
            ConsoleHelper.PrintLine("#Total package count: " + requestMessage.TotalPacketCount, ConsoleColor.DarkGray);
            ConsoleHelper.PrintLine("#Reason: " + responseMessage.Reason ?? "*None*", ConsoleColor.DarkGray);
            ConsoleHelper.PrintLine("#IsAccept: " + responseMessage.IsAccept, ConsoleColor.DarkGray);
            ConsoleHelper.PrintLine("#Service name: " + requestMessage.ServiceName ?? "*None*", ConsoleColor.DarkGray);
            ConsoleHelper.PrintLine("#Name: " + requestMessage.Name ?? "*None*", ConsoleColor.DarkGray);
            ConsoleHelper.PrintLine("#Version: " + requestMessage.Version ?? "*None*", ConsoleColor.DarkGray);
            ConsoleHelper.PrintLine("#Description: " + requestMessage.Description ?? "*None*", ConsoleColor.DarkGray);
            return responseMessage;
        }

        #endregion
    }
}