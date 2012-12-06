using System;
using KJFramework.IO.Helper;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.DSN.CP.Deployer.Deployers;
using KJFramework.Platform.Deploy.DSN.CP.Deployer.Deployers.Steps;
using KJFramework.Platform.Deploy.DSN.CP.Deployer.Packages;
using KJFramework.Platform.Deploy.DSN.ProtocolStack;

namespace KJFramework.Platform.Deploy.DSN.CP.Deployer.Processors
{
    /// <summary>
    ///     开始部署服务请求消息处理器
    /// </summary>
    public class DSNBeginDeployMessageProcessor : FunctionProcessor<DSNMessage>
    {
        #region Overrides of FunctionProcessor<DSNMessage>

        public override DSNMessage Process(Guid id, DSNMessage message)
        {
            DSNBeginDeployMessage msg = (DSNBeginDeployMessage)message;
            ConsoleHelper.PrintLine("=>\r\nReceived a new message of Begin Deploy Request Message, Details below: ", ConsoleColor.DarkGray);
            ConsoleHelper.PrintLine("#Request token: " + msg.RequestToken, ConsoleColor.DarkGray);
            ConsoleHelper.PrintLine("#ReportDetail: " + msg.ReportDetail, ConsoleColor.DarkGray);
            IFilePackage package = DataBus.GetPackage(msg.RequestToken);
            if (package == null)
            {
                ConsoleHelper.PrintLine("[WARNING]Can't process this end transfer file request message. #token: " + msg.RequestToken + ", beacuse no any package reference this token.", ConsoleColor.DarkYellow);
                return new DSNEndDeployMessage
                           {
                               IsDeployed = false,
                               RequestToken = msg.RequestToken,
                               LastError = "Can't process this begin deploy file request message. "
                           };
            }
            int[] loseIds = package.CheckComplate();
            if (loseIds != null)
            {
                return new DSNEndDeployMessage
                {
                    IsDeployed = false,
                    RequestToken = msg.RequestToken,
                    LastError = "Can't process this begin deploy file, beacuse this file package still no complated."
                };
            }
            DynamicServiceDeployer deployer = new DynamicServiceDeployer(msg.RequestToken, package, id, msg.ReportDetail);
            deployer.Add(new SaveBinaryFileDeployStep());
            deployer.Add(new DecompressRarDeployStep());
            deployer.Add(new CreateWindowServiceDeployStep());
            bool result = deployer.Deploy(new[] {package});
            return new DSNEndDeployMessage
            {
                IsDeployed = result,
                RequestToken = msg.RequestToken,
                LastError = deployer.LastException != null ? deployer.LastException.Message : null
            };
        }

        #endregion
    }
}