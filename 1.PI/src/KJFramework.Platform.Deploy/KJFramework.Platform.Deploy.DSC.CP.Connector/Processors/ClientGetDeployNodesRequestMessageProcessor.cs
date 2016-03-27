using System;
using KJFramework.IO.Helper;
using KJFramework.Logger;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Client.ProtocolStack;

namespace KJFramework.Platform.Deploy.DSC.CP.Connector.Processors
{
    public class ClientGetDeployNodesRequestMessageProcessor : FunctionProcessor<ClientMessage>
    {
        #region Overrides of FunctionProcessor<ClientMessage>

        public override ClientMessage Process(Guid id, ClientMessage message)
        {
            try
            {
                ClientGetDeployNodesResponseMessage rspMessage = new ClientGetDeployNodesResponseMessage();
                rspMessage.Header.ClientTag = message.Header.ClientTag;
                rspMessage.Header.TaskId = message.Header.TaskId;
                rspMessage.Items = DynamicServices.GetDeployNodes();
                return rspMessage;
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
                ConsoleHelper.PrintLine("Can not transport a object to component tunnel. Error message: \r\n" + ex.Message, ConsoleColor.DarkRed);
            }
            return null;
        }

        #endregion
    }
}