using System;
using KJFramework.IO.Helper;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.DSN.CP.Deployer.Packages;
using KJFramework.Platform.Deploy.DSN.ProtocolStack;

namespace KJFramework.Platform.Deploy.DSN.CP.Deployer.Processors
{
    /// <summary>
    ///     传输数据消息处理器
    /// </summary>
    public class DSNTransferDataMessageProcessor : FunctionProcessor<DSNMessage>
    {
        #region Overrides of FunctionProcessor<DSNMessage>

        public override DSNMessage Process(Guid id, DSNMessage message)
        {
            DSNTransferDataMessage msg = (DSNTransferDataMessage) message;
            ConsoleHelper.PrintLine("#New transfer data message, #token: " + msg.RequestToken + ", current package id: " + msg.CurrentPackageId + ", data length: " + msg.Data.Length, ConsoleColor.DarkGray);
            IFilePackage package =  DataBus.GetPackage(msg.RequestToken);
            if (package == null)
            {
                ConsoleHelper.PrintLine("[WARNING]Can't process this transfer data message. #token: " + msg.RequestToken + ", beacuse no any package reference this token.", ConsoleColor.DarkYellow);
                return null;
            }
            package.Add(new FileData(msg.Data, msg.CurrentPackageId));
            return null;
        }

        #endregion
    }
}