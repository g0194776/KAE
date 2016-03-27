using System;
using KJFramework.IO.Helper;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.DSN.CP.Deployer.Deployers;
using KJFramework.Platform.Deploy.DSN.CP.Deployer.Deployers.Steps;
using KJFramework.Platform.Deploy.DSN.ProtocolStack;

namespace KJFramework.Platform.Deploy.DSN.CP.Deployer.Processors
{
    /// <summary>
    ///     开启服务请求消息处理器
    /// </summary>
    public class DSNStartServiceRequestMessageProcessor : FunctionProcessor<DSNMessage>
    {
        public override DSNMessage Process(Guid id, DSNMessage message)
        {
            DSNStartServiceRequestMessage msg = (DSNStartServiceRequestMessage) message;
            DSNStartServiceResponseMessage responseMessage = new DSNStartServiceResponseMessage();
            ConsoleHelper.PrintLine("=>\r\nReceived a new message of Start Service Request Message, Details below: ", ConsoleColor.DarkGray);
            ConsoleHelper.PrintLine("#Service name: " + msg.ServiceName, ConsoleColor.DarkGray);
            DynamicServiceDeployer deployer = new DynamicServiceDeployer("NONE-REQUEST-TOKEN", null, id);
            deployer.Add(new StartWindowServiceDeployStep());
            bool result = deployer.Deploy(msg.ServiceName);
            responseMessage.ServiceName = msg.ServiceName;
            responseMessage.MachineName = Global.MachineName;
            responseMessage.IsSuccess = result;
            responseMessage.LastError = deployer.LastException != null ? deployer.LastException.Message : null;
            return responseMessage;
        }
    }
}