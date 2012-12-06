using System;
using KJFramework.IO.Helper;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.DSN.CP.Deployer.Packages;
using KJFramework.Platform.Deploy.DSN.ProtocolStack;

namespace KJFramework.Platform.Deploy.DSN.CP.Deployer.Processors
{
    /// <summary>
    ///     ����������Ϣ������
    /// </summary>
    public class DSNLossCompensationMessageProcessor : FunctionProcessor<DSNMessage>
    {
        #region Overrides of FunctionProcessor<DSNMessage>

        public override DSNMessage Process(Guid id, DSNMessage message)
        {
            DSNLossCompensationMessage msg = (DSNLossCompensationMessage) message;
            ConsoleHelper.PrintLine("#New loss compensation data message, #token: " + msg.RequestToken + ", current package id: " + msg.CurrentPackageId + ", data length: " + msg.Data.Length, ConsoleColor.DarkGray);
            IFilePackage package = DataBus.GetPackage(msg.RequestToken);
            if (package == null)
            {
                ConsoleHelper.PrintLine("[WARNING]Can't process this loss compensation data message. #token: " + msg.RequestToken + ", beacuse no any package reference this token.", ConsoleColor.DarkYellow);
                return null;
            }
            package.Add(new FileData(msg.Data, msg.CurrentPackageId));
            return null;
        }

        #endregion
    }
}