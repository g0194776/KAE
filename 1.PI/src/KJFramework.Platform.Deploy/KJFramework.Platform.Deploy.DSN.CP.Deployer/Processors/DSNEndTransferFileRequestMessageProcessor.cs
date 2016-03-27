using System;
using KJFramework.IO.Helper;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.DSN.CP.Deployer.Packages;
using KJFramework.Platform.Deploy.DSN.ProtocolStack;

namespace KJFramework.Platform.Deploy.DSN.CP.Deployer.Processors
{
    /// <summary>
    ///     结束传输文件请求消息处理器
    /// </summary>
    public class DSNEndTransferFileRequestMessageProcessor : FunctionProcessor<DSNMessage>
    {
        #region Overrides of FunctionProcessor<DSNMessage>

        public override DSNMessage Process(Guid id, DSNMessage message)
        {
            DSNEndTransferFileRequestMessage requestMessage = (DSNEndTransferFileRequestMessage)message;
            DSNEndTransferFileResponseMessage responseMessage = new DSNEndTransferFileResponseMessage();
            responseMessage.RequestToken = requestMessage.RequestToken;
            IFilePackage package = DataBus.GetPackage(requestMessage.RequestToken);
            if (package == null)
            {
                ConsoleHelper.PrintLine("[WARNING]Can't process this end transfer file request message. #token: " + requestMessage.RequestToken + ", beacuse no any package reference this token.", ConsoleColor.DarkYellow);
                responseMessage.IsDone = false;
            }
            else
            {
                int[] loseIds = package.CheckComplate();
                responseMessage.LosePackets = loseIds;
                responseMessage.IsDone = responseMessage.LosePackets == null;
            }
            ConsoleHelper.PrintLine("=>\r\nReceived a new message of End Transfer File Request Message, Details below: ", ConsoleColor.DarkGray);
            ConsoleHelper.PrintLine("#Request token: " + requestMessage.RequestToken, ConsoleColor.DarkGray);
            ConsoleHelper.PrintLine("#IsDone: " + responseMessage.IsDone, ConsoleColor.DarkGray);
            if (responseMessage.LosePackets != null)
            {
                ConsoleHelper.PrintLine("#Lose compensation count: " + responseMessage.LosePackets.Length, ConsoleColor.DarkGray);
            }
            return responseMessage;
        }

        #endregion
    }
}