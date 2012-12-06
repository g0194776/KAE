using System;
using KJFramework.IO.Helper;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.SMC.ProtocolStack.Services;

namespace KJFramework.Platform.Deploy.Maintenance.Processors
{
    /// <summary>
    ///     注册回馈消息处理器
    /// </summary>
    internal class DSRegistResponseMessageProcessor : FunctionProcessor<DynamicServiceMessage>
    {
        #region Overrides of FunctionProcessor<DynamicServiceMessage>

        public override DynamicServiceMessage Process(Guid id, DynamicServiceMessage message)
        {
            DynamicServiceRegistResponseMessage msg = (DynamicServiceRegistResponseMessage) message;
            if (msg.Result)
            {
                ConsoleHelper.PrintLine("Done !", ConsoleColor.DarkGreen);
            }
            else
            {
                ConsoleHelper.PrintLine("Failed !", ConsoleColor.DarkRed);
                ConsoleHelper.PrintLine("#WARING:\r\nRegist to remote management node failed, " + Global.DynamicService.Infomation.ServiceName + " service will be run at uncontrollable platform !", ConsoleColor.Yellow);
            }
            return null;
        }

        #endregion
    }
}