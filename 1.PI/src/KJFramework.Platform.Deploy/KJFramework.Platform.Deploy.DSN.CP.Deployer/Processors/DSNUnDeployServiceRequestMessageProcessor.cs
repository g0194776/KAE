using System;
using KJFramework.IO.Helper;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.DSN.CP.Deployer.Deployers;
using KJFramework.Platform.Deploy.DSN.CP.Deployer.Deployers.Steps;
using KJFramework.Platform.Deploy.DSN.ProtocolStack;

namespace KJFramework.Platform.Deploy.DSN.CP.Deployer.Processors
{
    /// <summary>
    ///     卸载服务请求消息处理器
    /// </summary>
    public class DSNUnDeployServiceRequestMessageProcessor : FunctionProcessor<DSNMessage>
    {
        #region Overrides of FunctionProcessor<DSNMessage>

        public override DSNMessage Process(Guid id, DSNMessage message)
        {
            DSNUnDeployServiceRequestMessage msg = (DSNUnDeployServiceRequestMessage) message;
            DSNUnDeployServiceResponseMessage responseMessage = new DSNUnDeployServiceResponseMessage();
            ConsoleHelper.PrintLine("=>\r\nReceived a new message of Un Deploy Service Request Message, Details below: ", ConsoleColor.DarkGray);
            ConsoleHelper.PrintLine("#Service name: " + msg.ServiceName, ConsoleColor.DarkGray);
            ConsoleHelper.PrintLine("#ReportDetail: " + msg.IsDetailReport, ConsoleColor.DarkGray);
            ConsoleHelper.PrintLine("#Reason: " + msg.Reason, ConsoleColor.DarkGray);
            DynamicServiceDeployer deployer = new DynamicServiceDeployer("NONE-REQUEST-TOKEN", null, id, msg.IsDetailReport);
            deployer.Add(new CheckWindowServiceExistsDeployStep());
            deployer.Add(new StopWindowServiceDeployStep());
            deployer.Add(new UnInstallServiceDeployStep());
            bool result = deployer.Deploy(msg.ServiceName);
            responseMessage.IsSuccess = result;
            responseMessage.ServiceName = msg.ServiceName;
            responseMessage.LastError = deployer.LastException != null ? deployer.LastException.Message : null;
            ConsoleHelper.PrintLine("#Response of Un Deploy Service Request Message, Details below: ", ConsoleColor.DarkGray);
            ConsoleHelper.PrintLine("#Service name: " + responseMessage.ServiceName, ConsoleColor.DarkGray);
            ConsoleHelper.PrintLine("#IsSuccess: " + responseMessage.IsSuccess, ConsoleColor.DarkGray);
            ConsoleHelper.PrintLine("#Last error: " + responseMessage.LastError ?? "*None*", ConsoleColor.DarkGray);
            return responseMessage;
        }

        #endregion
    }
}