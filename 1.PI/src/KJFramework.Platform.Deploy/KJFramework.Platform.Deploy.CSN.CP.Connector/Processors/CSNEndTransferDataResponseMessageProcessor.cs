using System;
using KJFramework.IO.Helper;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Enums;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Transmitters;
using KJFramework.Platform.Deploy.CSN.ProtocolStack;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector.Processors
{
    /// <summary>
    ///     结束传输数据回馈消息处理器
    /// </summary>
    public class CSNEndTransferDataResponseMessageProcessor : FunctionProcessor<CSNMessage>
    {
        #region Overrides of FunctionProcessor<CSNMessage>

        public override CSNMessage Process(Guid id, CSNMessage message)
        {
            CSNEndTransferDataResponseMessage msg = (CSNEndTransferDataResponseMessage) message;
            ConsoleHelper.PrintLine("#Received a end transfer data response message, details below:", ConsoleColor.DarkGray);
            ConsoleHelper.PrintLine("#Serial number:" + msg.SerialNumber, ConsoleColor.DarkGray);
            ConsoleHelper.PrintLine("#IsComplated:" + msg.IsComplated, ConsoleColor.DarkGray);
            IConfigTransmitter configTransmitter = ConfigTransmitterManager.Instance.GetTransmitter(msg.SerialNumber);
            if (configTransmitter == null)
            {
                ConsoleHelper.PrintLine("#Can not get a configuration transmitter, #id: " + msg.SerialNumber, ConsoleColor.DarkRed);
            }
            else
            {
                configTransmitter.NextStep = msg.IsComplated ? TransmitterSteps.Finish : TransmitterSteps.TransferData;
                configTransmitter.Next(msg.PackageIds);
            }
            return null;
        }

        #endregion
    }
}