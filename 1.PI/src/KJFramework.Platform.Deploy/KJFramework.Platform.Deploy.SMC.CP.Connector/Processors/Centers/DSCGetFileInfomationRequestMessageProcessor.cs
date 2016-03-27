using System;
using KJFramework.IO.Helper;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;

namespace KJFramework.Platform.Deploy.SMC.CP.Connector.Processors.Centers
{
    /// <summary>
    ///     ��ȡ�ļ���ϸ��Ϣ������Ϣ������
    /// </summary>
    public class DSCGetFileInfomationRequestMessageProcessor : FunctionProcessor<DSCMessage>
    {
        #region Overrides of FunctionProcessor<DSCMessage>

        public override DSCMessage Process(Guid id, DSCMessage message)
        {
            DSCGetFileInfomationRequestMessage msg = (DSCGetFileInfomationRequestMessage) message;
            bool result = ServicePerformancer.Instance.GetFileInfos(msg);
            if (!result)
            {
                DSCMessage  responseMessage = new DSCUpdateComponentResponseMessage
                {
                    ErrorTrace = "�޷���һ����ȡ�ļ���ϸ��Ϣ������֪ͨ��һ�������ϣ���Ϊ��������޷�������Ŀ����� ! #Service :" + msg.ServiceName,
                    Result = false
                };
                responseMessage.Header.ClientTag = msg.Header.ClientTag;
                return responseMessage;
            }
            ConsoleHelper.PrintLine("�Ѿ��ɹ��Ľ���ȡ�ļ���ϸ��Ϣ����Ϣ֪ͨ����Ŀ����� ! #Service: " + msg.ServiceName, ConsoleColor.DarkGreen);
            return null;
        }

        #endregion
    }
}