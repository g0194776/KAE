using System;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.SMC.ProtocolStack.Services;

namespace KJFramework.Platform.Deploy.Maintenance.Processors
{
    /// <summary>
    ///     ��ȡ�ļ���ϸ��Ϣ������Ϣ������
    /// </summary>
    public class DSGetFileInfomationRequestMessageProcessor : FunctionProcessor<DynamicServiceMessage>
    {
        #region Overrides of FunctionProcessor<DynamicServiceMessage>

        public override DynamicServiceMessage Process(Guid id, DynamicServiceMessage message)
        {
            DynamicServiceGetFileInfomationRequestMessage msg = (DynamicServiceGetFileInfomationRequestMessage) message;
            return Global.DynamicService.GetFileInfomation(msg);
        }

        #endregion
    }
}