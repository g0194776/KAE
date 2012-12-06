using System;
using KJFramework.IO.Helper;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Enums;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Transmitters;
using KJFramework.Platform.Deploy.CSN.ProtocolStack;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector.Processors
{
    /// <summary>
    ///     开始传输数据回馈包处理器
    /// </summary>
    public class CSNBeginTransferDataResponseMessageProcessor : FunctionProcessor<CSNMessage>
    {
        #region Overrides of FunctionProcessor<CSNMessage>

        public override CSNMessage Process(Guid id, CSNMessage message)
        {
            CSNBeginTransferDataResponseMessage msg = (CSNBeginTransferDataResponseMessage)message;
            ConsoleHelper.PrintLine("#Received a begin transfer data response message, details below:", ConsoleColor.DarkGray);
            ConsoleHelper.PrintLine("#Serial number:" + msg.SerialNumber, ConsoleColor.DarkGray);
            ConsoleHelper.PrintLine("#IsAccept:" + msg.IsAccept, ConsoleColor.DarkGray);
            IConfigTransmitter configTransmitter = ConfigTransmitterManager.Instance.GetTransmitter(msg.SerialNumber);
            if (configTransmitter == null)
            {
                ConsoleHelper.PrintLine("#Can not get a configuration transmitter, #id: " + msg.SerialNumber, ConsoleColor.DarkRed);
            }
            else
            {
                configTransmitter.NextStep = msg.IsAccept ? TransmitterSteps.TransferData : TransmitterSteps.Finish;
                configTransmitter.Next();
            }
            return null;
        }

        #endregion
    }
}