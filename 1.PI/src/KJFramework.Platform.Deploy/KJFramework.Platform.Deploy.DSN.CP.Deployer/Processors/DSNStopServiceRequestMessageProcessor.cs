using System;
using KJFramework.IO.Helper;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.DSN.CP.Deployer.Deployers;
using KJFramework.Platform.Deploy.DSN.CP.Deployer.Deployers.Steps;
using KJFramework.Platform.Deploy.DSN.ProtocolStack;

namespace KJFramework.Platform.Deploy.DSN.CP.Deployer.Processors
{
    /// <summary>
    ///     停止服务请求消息处理器
    /// </summary>
    public class DSNStopServiceRequestMessageProcessor : FunctionProcessor<DSNMessage>
    {
        public override DSNMessage Process(Guid id, DSNMessage message)
        {
            DSNStopServiceRequestMessage msg = (DSNStopServiceRequestMessage)message;
            DSNStopServiceResponseMessage responseMessage = new DSNStopServiceResponseMessage();
            ConsoleHelper.PrintLine("=>\r\nReceived a new message of Stop Service Request Message, Details below: ", ConsoleColor.DarkGray);
            ConsoleHelper.PrintLine("#Service name: " + msg.ServiceName, ConsoleColor.DarkGray);
            DynamicServiceDeployer deployer = new DynamicServiceDeployer("NONE-REQUEST-TOKEN", null, id);
            deployer.Add(new CheckWindowServiceExistsDeployStep());
            deployer.Add(new StopWindowServiceDeployStep());
            bool result = deployer.Deploy(msg.ServiceName);
            responseMessage.MachineName = Global.MachineName;
            responseMessage.ServiceName = msg.ServiceName;
            responseMessage.IsSuccess = result;
            responseMessage.LastError = deployer.LastException != null ? deployer.LastException.Message : null;
            return responseMessage;
        }
    }
}