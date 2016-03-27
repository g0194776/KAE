using System;
using KJFramework.IO.Helper;
using KJFramework.Logger;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Client.ProtocolStack;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;
using KJFramework.Platform.Deploy.Metadata.Objects;
using KJFramework.Platform.Deploy.SMC.Common.Performances;
using KJFramework.Platform.DSC.ProtocolStack;

namespace KJFramework.Platform.Deploy.DSC.CP.Connector.Processors
{
    /// <summary>
    ///     DSC获取服务详细信息的回馈消息处理器
    /// </summary>
    public class DSCGetServicesResponseMessageProcessor : FunctionProcessor<DSCMessage>
    {
        #region Overrides of FunctionProcessor<DSCMessage>

        public override DSCMessage Process(Guid id, DSCMessage message)
        {
            DSCGetServicesResponseMessage responseMessage = (DSCGetServicesResponseMessage)message;
            ConsoleHelper.PrintLine("=>\r\nGet services infomation response, details below: ", ConsoleColor.DarkGray);
            ConsoleHelper.PrintLine("Machine name: " + responseMessage.MachineName, ConsoleColor.DarkGray);
            if (responseMessage.Items != null)
            {
                foreach (OwnServiceItem ownServiceItem in responseMessage.Items)
                {
                    ConsoleHelper.PrintLine("  #Service full name: " + ownServiceItem.ServiceName, ConsoleColor.DarkGray);
                    ConsoleHelper.PrintLine("  #Service component count: " + ownServiceItem.ComponentCount, ConsoleColor.DarkGray);
                    ConsoleHelper.PrintLine("  #Service live Status: " + ownServiceItem.LiveStatus, ConsoleColor.DarkGray);
                    ConsoleHelper.PrintLine("  #Service description: " + (ownServiceItem.Description ?? "*None*"), ConsoleColor.DarkGray);
                    ConsoleHelper.PrintLine("  #Service version: " + (ownServiceItem.Version ?? "*None*"), ConsoleColor.DarkGray);
                    ConsoleHelper.PrintLine("  #Service name: " + (ownServiceItem.Name ?? "*None*"), ConsoleColor.DarkGray);
                    ConsoleHelper.PrintLine("  #Service control address: " + (ownServiceItem.ControlServiceAddress ?? "*None*"), ConsoleColor.DarkGray);
                    ConsoleHelper.PrintLine("  #Service last heartbeat time: " + ownServiceItem.LastHeartbeatTime, ConsoleColor.DarkGray);
                    ConsoleHelper.PrintLine("  #Service last update time: " + ownServiceItem.LastUpdateTime, ConsoleColor.DarkGray);
                    ConsoleHelper.PrintLine("  #Service support appdomain performance: " + ownServiceItem.SupportDomainPerformance, ConsoleColor.DarkGray);
                    if (ownServiceItem.PerformanceItems != null)
                    {
                        foreach (ServicePerformanceItem item in ownServiceItem.PerformanceItems)
                        {
                            ConsoleHelper.PrintLine(String.Format("  #Performance {0}: {1}", item.PerformanceName, item.PerformanceValue), ConsoleColor.DarkGray);
                        }
                    }
                    if (ownServiceItem.DomainPerformanceItems != null)
                    {
                        foreach (DomainPerformanceItem item in ownServiceItem.DomainPerformanceItems)
                        {
                            ConsoleHelper.PrintLine(String.Format("  #Domain Performance {0}: {1}", item.Cpu, item.Memory), ConsoleColor.DarkGray);
                        }
                    }
                    if (ownServiceItem.Componnets != null)
                    {
                        foreach (OwnComponentItem componentItem in ownServiceItem.Componnets)
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
            DynamicServices.Update(responseMessage);
            #region Notify to client

            try
            {
                ClientGetServicesResponseMessage clientMessage = new ClientGetServicesResponseMessage();
                clientMessage.Header.ClientTag = responseMessage.Header.ClientTag ?? "*ALL*";
                clientMessage.Header.TaskId = responseMessage.Header.TaskId ?? clientMessage.Header.TaskId;
                clientMessage.Items = responseMessage.Items;
                clientMessage.MachineName = responseMessage.MachineName;
                clientMessage.Bind();
                Guid[] channelIds = Client.GetClient(clientMessage.Header.ClientTag);
                if (channelIds != null)
                {
                    foreach (Guid guid in channelIds)
                        Global.CommnicationNode.Send(guid, clientMessage.Body);
                }
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
                ConsoleHelper.PrintLine("Can not transport a object to component tunnel. Error message: \r\n" + ex.Message, ConsoleColor.DarkRed);
            }
            return null;

            #endregion
        }

        #endregion
    }
}