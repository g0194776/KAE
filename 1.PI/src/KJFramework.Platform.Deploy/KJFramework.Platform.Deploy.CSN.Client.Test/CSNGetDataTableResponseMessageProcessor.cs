using System;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.CSN.ProtocolStack;

namespace KJFramework.Platform.Deploy.CSN.Client.Test
{
    public class CSNGetDataTableResponseMessageProcessor : FunctionProcessor<CSNMessage>
    {
        #region Overrides of FunctionProcessor<CSNMessage>

        public override CSNMessage Process(Guid id, CSNMessage message)
        {
            CSNGetDataTableResponseMessage responseMessage = (CSNGetDataTableResponseMessage) message;
            Console.WriteLine("#Get table data response message received, details below: ");
            Console.WriteLine("#LastError: " + responseMessage.LastError ?? "*None*");
            Console.WriteLine("#Result: " + responseMessage == null ? "True" : "False");
            return null;
        }

        #endregion
    }
}