using System;
using KJFramework.IO.Helper;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;
using KJFramework.Platform.Deploy.Metadata.Objects;
using KJFramework.Platform.Deploy.SMC.Common.Performances;

namespace KJFramework.Platform.Deploy.DSC.CP.Connector.Processors
{
    /// <summary>
    ///     DSC注册请求消息处理器
    /// </summary>
    public class DSCRegistRequestMessageProcessor : FunctionProcessor<DSCMessage>
    {
        #region Overrides of FunctionProcessor<DSCMessage>

        public override DSCMessage Process(Guid id, DSCMessage message)
        {
            DSCRegistRequestMessage msg = (DSCRegistRequestMessage) message;
            ConsoleHelper.PrintLine("New sub machine controller registing......Details below: ", ConsoleColor.DarkGray);
            ConsoleHelper.PrintLine("Machine Name: " + msg.MachineName, ConsoleColor.DarkGray);
            if (msg.OwnServics != null)
            {
                ConsoleHelper.PrintLine("Service detail infomation......", ConsoleColor.DarkGray);
                foreach (OwnServiceItem item in msg.OwnServics)
                {
                    if (item.Name != null)
                    {
                        ConsoleHelper.PrintLine("Name: " + item.Name, ConsoleColor.DarkGray);
                    }
                    if (item.Version != null)
                    {
                        ConsoleHelper.PrintLine("Version: " + item.Version, ConsoleColor.DarkGray);
                    }
                    if (item.Description != null)
                    {
                        ConsoleHelper.PrintLine("Description: " + item.Description, ConsoleColor.DarkGray);
                    }
                    ConsoleHelper.PrintLine("Service Name: " + item.ServiceName, ConsoleColor.DarkGray);
                    ConsoleHelper.PrintLine("Component Count: " + item.ComponentCount, ConsoleColor.DarkGray);
                    if (item.Componnets != null)
                    {
                        foreach (OwnComponentItem componentItem in item.Componnets)
                        {
                            ConsoleHelper.PrintLine("    #Component full name: " + componentItem.ComponentName, ConsoleColor.DarkGray);
                            ConsoleHelper.PrintLine("    #Component status: " + componentItem.Status, ConsoleColor.DarkGray);
                            ConsoleHelper.PrintLine("    #Component name: " + (componentItem.Name ?? "*None*"), ConsoleColor.DarkGray);
                            ConsoleHelper.PrintLine("    #Component description: " + (componentItem.Description ?? "*None*"), ConsoleColor.DarkGray);
                            ConsoleHelper.PrintLine("    #Component version: " + (componentItem.Version ?? "*None*"), ConsoleColor.DarkGray);
                            ConsoleHelper.PrintLine("    #Component last update time: " + componentItem.LastUpdateTime, ConsoleColor.DarkGray);
                        }
                    }
                }
            }
            if (msg.PerformanceItems != null)
            {
                ConsoleHelper.PrintLine("Performance detail infomation......", ConsoleColor.DarkGray);
                foreach (ServicePerformanceItem item in msg.PerformanceItems)
                {
                    ConsoleHelper.PrintLine(String.Format("{0}: {1}", item.PerformanceName, item.PerformanceValue), ConsoleColor.DarkGray);
                }
            }
            if (msg.DeployAddress != null)
            {
                ConsoleHelper.PrintLine("Deploy Address: " + msg.DeployAddress, ConsoleColor.DarkGray);
            }
            if (msg.ControlAddress != null)
            {
                ConsoleHelper.PrintLine("Control Address: " + msg.ControlAddress, ConsoleColor.DarkGray);
            }
            return new DSCRegistResponseMessage {Result = DynamicServices.Regist(msg, id)};
        }

        #endregion
    }
}