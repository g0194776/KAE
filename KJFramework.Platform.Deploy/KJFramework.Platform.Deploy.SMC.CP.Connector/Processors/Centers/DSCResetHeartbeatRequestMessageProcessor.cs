using System;
using KJFramework.IO.Helper;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;

namespace KJFramework.Platform.Deploy.SMC.CP.Connector.Processors.Centers
{
    /// <summary>
    ///     DSC重置心跳时间请求消息处理器
    /// </summary>
    public class DSCResetHeartbeatRequestMessageProcessor : FunctionProcessor<DSCMessage>
    {
        #region Overrides of FunctionProcessor<DSCMessage>

        public override DSCMessage Process(Guid id, DSCMessage message)
        {
            DSCResetHeartBeatTimeRequestMessage msg = (DSCResetHeartBeatTimeRequestMessage) message;
            //设置SMC自身的心跳时间
            if (msg.Target == "*SMC*")
            {
                Global.ConnectorInstance.ResetHeartbeatTime(msg.Interval);
            }
            else
            {
                ConsoleHelper.PrintLine("#Already reset dynamic service heartbeat interval. value: " + msg.Interval, ConsoleColor.DarkGreen);
                ServicePerformancer.Instance.ResetServiceHeartbeatTime(msg.Interval);
            }
            DSCMessage responseMessage=  new DSCResetHeartBeatTimeResponseMessage { Result = true, MachineName = Environment.MachineName };
            responseMessage.Header.ClientTag = msg.Header.ClientTag;
            return responseMessage;
        }

        #endregion
    }
}