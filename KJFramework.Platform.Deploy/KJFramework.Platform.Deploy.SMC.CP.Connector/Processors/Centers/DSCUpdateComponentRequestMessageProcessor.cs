using System;
using KJFramework.IO.Helper;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;

namespace KJFramework.Platform.Deploy.SMC.CP.Connector.Processors.Centers
{
    /// <summary>
    ///     DSC�������������Ϣ
    /// </summary>
    public class DSCUpdateComponentRequestMessageProcessor : FunctionProcessor<DSCMessage>
    {
        #region Overrides of FunctionProcessor<DSCMessage>

        public override DSCMessage Process(Guid id, DSCMessage message)
        {
            DSCUpdateComponentRequestMessage msg = (DSCUpdateComponentRequestMessage) message;
            bool result = ServicePerformancer.Instance.Update(msg);
            if (!result)
            {
                DSCMessage responseMessage =  new DSCUpdateComponentResponseMessage
                           {
                               ErrorTrace = "�޷���һ���������������֪ͨ��һ�������ϣ���Ϊ��������޷�������Ŀ����� ! #Service :" + msg.ServiceName,
                               Result = false
                           };
                responseMessage.Header.ClientTag = msg.Header.ClientTag;
                return responseMessage;
            }
            ConsoleHelper.PrintLine("�Ѿ��ɹ��Ľ�������µ���Ϣ֪ͨ����Ŀ����� ! #Service: " + msg.ServiceName, ConsoleColor.DarkGreen);
            return null;
        }

        #endregion
    }
}