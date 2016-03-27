using System;
using KJFramework.IO.Helper;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;

namespace KJFramework.Platform.Deploy.SMC.CP.Connector.Processors.Centers
{
    /// <summary>
    ///     DSC获取组件健康状态请求消息处理器
    /// </summary>
    public class DSCGetComponentHealthRequestMessageProcessor : FunctionProcessor<DSCMessage>
    {
        #region Overrides of FunctionProcessor<DSCMessage>

        public override DSCMessage Process(Guid id, DSCMessage message)
        {
            DSCGetComponentHealthRequestMessage msg = (DSCGetComponentHealthRequestMessage) message;
            bool result = ServicePerformancer.Instance.Update(msg);
            if (!result)
            {
                DSCMessage responseMessage =  new DSCGetComponentHealthResponseMessage
                           {
                               ServiceName = msg.ServiceName,
                               LastError = "无法获取指定服务的组件健康状态，可能无法连接到目标服务。#Service: " + msg.ServiceName
                           };
                responseMessage.Header.ClientTag = msg.Header.ClientTag;
                return responseMessage;
            }
            ConsoleHelper.PrintLine("已经成功的将获取组件健康状态的消息通知到了目标服务 ! #Service: " + msg.ServiceName, ConsoleColor.DarkGreen);
            return null;
        }

        #endregion
    }
}