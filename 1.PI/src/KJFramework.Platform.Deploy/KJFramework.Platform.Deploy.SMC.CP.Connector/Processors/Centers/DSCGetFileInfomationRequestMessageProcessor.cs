using System;
using KJFramework.IO.Helper;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;

namespace KJFramework.Platform.Deploy.SMC.CP.Connector.Processors.Centers
{
    /// <summary>
    ///     获取文件详细信息请求消息处理器
    /// </summary>
    public class DSCGetFileInfomationRequestMessageProcessor : FunctionProcessor<DSCMessage>
    {
        #region Overrides of FunctionProcessor<DSCMessage>

        public override DSCMessage Process(Guid id, DSCMessage message)
        {
            DSCGetFileInfomationRequestMessage msg = (DSCGetFileInfomationRequestMessage) message;
            bool result = ServicePerformancer.Instance.GetFileInfos(msg);
            if (!result)
            {
                DSCMessage  responseMessage = new DSCUpdateComponentResponseMessage
                {
                    ErrorTrace = "无法将一个获取文件详细信息的请求通知到一个服务上，因为这个可能无法连接上目标服务 ! #Service :" + msg.ServiceName,
                    Result = false
                };
                responseMessage.Header.ClientTag = msg.Header.ClientTag;
                return responseMessage;
            }
            ConsoleHelper.PrintLine("已经成功的将获取文件详细信息的消息通知到了目标服务 ! #Service: " + msg.ServiceName, ConsoleColor.DarkGreen);
            return null;
        }

        #endregion
    }
}