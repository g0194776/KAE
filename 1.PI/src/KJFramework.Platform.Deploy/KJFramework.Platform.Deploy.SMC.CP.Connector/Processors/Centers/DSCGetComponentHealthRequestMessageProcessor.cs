using System;
using KJFramework.IO.Helper;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;

namespace KJFramework.Platform.Deploy.SMC.CP.Connector.Processors.Centers
{
    /// <summary>
    ///     DSC��ȡ�������״̬������Ϣ������
    /// </summary>
    public class DSCGetComponentHealthRequestMessageProcessor : FunctionProcessor<DSCMessage>
    {
        #region Overrides of FunctionProcessor<DSCMessage>

        public override DSCMessage Process(Guid id, DSCMessage message)
        {
            DSCGetComponentHealthRequestMessage msg = (DSCGetComponentHealthRequestMessage) message;
            bool result = ServicePerformancer.Instance.Update(msg);
            if (!result)
            {
                DSCMessage responseMessage =  new DSCGetComponentHealthResponseMessage
                           {
                               ServiceName = msg.ServiceName,
                               LastError = "�޷���ȡָ��������������״̬�������޷����ӵ�Ŀ�����#Service: " + msg.ServiceName
                           };
                responseMessage.Header.ClientTag = msg.Header.ClientTag;
                return responseMessage;
            }
            ConsoleHelper.PrintLine("�Ѿ��ɹ��Ľ���ȡ�������״̬����Ϣ֪ͨ����Ŀ����� ! #Service: " + msg.ServiceName, ConsoleColor.DarkGreen);
            return null;
        }

        #endregion
    }
}