using System;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.CSN.ProtocolStack;

namespace KJFramework.Platform.Deploy.CSN.Client.Test
{
    public class CSNRegistResponseMessageProcessor : FunctionProcessor<CSNMessage>
    {
        #region Overrides of FunctionProcessor<CSNMessage>

        public override CSNMessage Process(Guid id, CSNMessage message)
        {
            CSNRegistResponseMessage responseMessage = (CSNRegistResponseMessage) message;
            Console.WriteLine("#Regist to CSN, result: " + responseMessage.Result);
            return null;
        }

        #endregion
    }
}