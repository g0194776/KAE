using System;
using KJFramework.IO.Helper;
using KJFramework.Logger;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Client.ProtocolStack;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;
using KJFramework.Platform.Deploy.Metadata.Objects;

namespace KJFramework.Platform.Deploy.DSC.CP.Connector.Processors
{
    /// <summary>
    ///     DSC状态变更请求消息处理器
    /// </summary>
    public class DSCStatusChangeRequestMessageProcessor : FunctionProcessor<DSCMessage>
    {
        #region Overrides of FunctionProcessor<DSCMessage>

        public override DSCMessage Process(Guid id, DSCMessage message)
        {
            DSCStatusChangeRequestMessage msg = (DSCStatusChangeRequestMessage) message;
            ConsoleHelper.PrintLine("=>\r\nReceived a status change request message, Details below:", ConsoleColor.DarkGray);
            ConsoleHelper.PrintLine("Machine name: " + msg.MachineName, ConsoleColor.DarkGray);
            ConsoleHelper.PrintLine("Status change type: " + msg.StatusChangeType, ConsoleColor.DarkGray);
            if (msg.Items != null)
            {
                foreach (OwnServiceItem ownServiceItem in msg.Items)
                {
                    ConsoleHelper.PrintLine("  #Service full name: " + ownServiceItem.ServiceName, ConsoleColor.DarkGray);
                    ConsoleHelper.PrintLine("  #Service component count: " + ownServiceItem.ComponentCount, ConsoleColor.DarkGray); 
                    ConsoleHelper.PrintLine("  #Service live Status: " + ownServiceItem.LiveStatus, ConsoleColor.DarkGray);
                    ConsoleHelper.PrintLine("  #Service description: " + (ownServiceItem.Description ?? "*None*"), ConsoleColor.DarkGray);
                    ConsoleHelper.PrintLine("  #Service version: " + (ownServiceItem.Version ?? "*None*"), ConsoleColor.DarkGray);
                    ConsoleHelper.PrintLine("  #Service shell version: " + (ownServiceItem.ShellVersion ?? "*None*"), ConsoleColor.DarkGray);
                    ConsoleHelper.PrintLine("  #Service name: " + (ownServiceItem.Name ?? "*None*"), ConsoleColor.DarkGray);
                    ConsoleHelper.PrintLine("  #Service control address: " + (ownServiceItem.ControlServiceAddress ?? "*None*"), ConsoleColor.DarkGray);
                    ConsoleHelper.PrintLine("  #Service last heartbeat time: " + ownServiceItem.LastHeartbeatTime, ConsoleColor.DarkGray);
                    ConsoleHelper.PrintLine("  #Service last update time: " + ownServiceItem.LastUpdateTime, ConsoleColor.DarkGray);
                    ConsoleHelper.PrintLine("  #Service support appdomain performance: " + ownServiceItem.SupportDomainPerformance, ConsoleColor.DarkGray);
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
            DynamicServices.Update(msg);
            #region Notify to client

            try
            {
                ClientStatusChangeMessage clientMessage = new ClientStatusChangeMessage();
                clientMessage.Header.ClientTag = msg.Header.ClientTag ?? "*ALL*";
                clientMessage.Header.TaskId = msg.Header.TaskId ?? clientMessage.Header.TaskId;
                clientMessage.Items = msg.Items;
                clientMessage.MachineName = msg.MachineName;
                clientMessage.StatusChangeType = msg.StatusChangeType;
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