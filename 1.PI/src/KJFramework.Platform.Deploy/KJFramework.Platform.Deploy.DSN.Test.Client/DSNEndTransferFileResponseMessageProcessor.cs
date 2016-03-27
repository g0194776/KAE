using System;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.DSN.ProtocolStack;

namespace KJFramework.Platform.Deploy.DSN.Test.Client
{
    public class DSNEndTransferFileResponseMessageProcessor : FunctionProcessor<DSNMessage>
    {
        #region Overrides of FunctionProcessor<DSNMessage>

        public override DSNMessage Process(Guid id, DSNMessage message)
        {
            DSNEndTransferFileResponseMessage responseMessage = (DSNEndTransferFileResponseMessage) message;
            if (responseMessage.IsDone)
            {
                Console.WriteLine("已经发起了开始部署服务的请求......");
                return new DSNBeginDeployMessage {ReportDetail = true, RequestToken = responseMessage.RequestToken};
            }
            return null;
        }

        #endregion
    }
}