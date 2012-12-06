using System;
using KJFramework.IO.Helper;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;

namespace KJFramework.Platform.Deploy.SMC.CP.Connector.Processors.Centers
{
    /// <summary>
    ///     DSC更新组件请求消息
    /// </summary>
    public class DSCUpdateComponentRequestMessageProcessor : FunctionProcessor<DSCMessage>
    {
        #region Overrides of FunctionProcessor<DSCMessage>

        public override DSCMessage Process(Guid id, DSCMessage message)
        {
            DSCUpdateComponentRequestMessage msg = (DSCUpdateComponentRequestMessage) message;
            bool result = ServicePerformancer.Instance.Update(msg);
            if (!result)
            {
                DSCMessage responseMessage =  new DSCUpdateComponentResponseMessage
                           {
                               ErrorTrace = "无法将一个更新组件的请求通知到一个服务上，因为这个可能无法连接上目标服务 ! #Service :" + msg.ServiceName,
                               Result = false
                           };
                responseMessage.Header.ClientTag = msg.Header.ClientTag;
                return responseMessage;
            }
            ConsoleHelper.PrintLine("已经成功的将组件更新的消息通知到了目标服务 ! #Service: " + msg.ServiceName, ConsoleColor.DarkGreen);
            return null;
        }

        #endregion
    }
}