using System;
using KJFramework.IO.Helper;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Transmitters;
using KJFramework.Platform.Deploy.CSN.ProtocolStack;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector.Processors
{
    /// <summary>
    ///     通知将要进行的多包传输回馈消息
    /// </summary>
    public class CSNMultiPackageDataNotifyResponseMessageProcessor : FunctionProcessor<CSNMessage>
    {
        #region Overrides of FunctionProcessor<CSNMessage>

        public override CSNMessage Process(Guid id, CSNMessage message)
        {
            CSNMultiPackageDataNotifyResponseMessage msg = (CSNMultiPackageDataNotifyResponseMessage)message;
            ConsoleHelper.PrintLine("#Received a multi package response message, details below:", ConsoleColor.DarkGray);
            ConsoleHelper.PrintLine("#Serial number:" + msg.SerialNumber, ConsoleColor.DarkGray);
            IConfigTransmitter configTransmitter = ConfigTransmitterManager.Instance.GetTransmitter(msg.SerialNumber);
            if (configTransmitter == null)
            {
                ConsoleHelper.PrintLine("#Can not get a configuration transmitter, #id: " + msg.SerialNumber, ConsoleColor.DarkRed);
            }
            else
            {
                configTransmitter.Next();
            }
            return null;
        }

        #endregion
    }
}