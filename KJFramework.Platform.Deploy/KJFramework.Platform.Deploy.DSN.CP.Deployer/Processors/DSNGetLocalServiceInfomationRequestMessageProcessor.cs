using System;
using KJFramework.IO.Helper;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.DSN.CP.Deployer.Deployers.Steps;
using KJFramework.Platform.Deploy.DSN.CP.Deployer.Reporters;
using KJFramework.Platform.Deploy.DSN.ProtocolStack;
using KJFramework.Platform.Deploy.Metadata.Objects;

namespace KJFramework.Platform.Deploy.DSN.CP.Deployer.Processors
{
    /// <summary>
    ///     获取本地服务相关信息请求消息处理器
    /// </summary>
    public class DSNGetLocalServiceInfomationRequestMessageProcessor : FunctionProcessor<DSNMessage>
    {
        #region Overrides of FunctionProcessor<DSNMessage>

        public override DSNMessage Process(Guid id, DSNMessage message)
        {
            ConsoleHelper.PrintLine("=>\r\nReceived a new message of Get Local Service Infomation Request Message......", ConsoleColor.DarkGray);
            DSNGetLocalServiceInfomationResponseMessage responseMessage = new DSNGetLocalServiceInfomationResponseMessage();
            responseMessage.MachineName = Global.MachineName;
            GetLocalServicesDeployStep deployStep = new GetLocalServicesDeployStep();
            deployStep.Reporter = new DeployStatusReporter(id, "NONE-REQUEST-TOKEN", true);
            Object[] context;
            deployStep.Execute(out context, null);
            if (deployStep.Exception == null)
            {
                responseMessage.Services = (ServiceInfoItem[])context[0];
            }
            else
            {
                responseMessage.LastError = deployStep.Exception == null ? null : deployStep.Exception.Message;
            }
            return responseMessage;
        }

        #endregion
    }
}