using System;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.SMC.ProtocolStack.Services;

namespace KJFramework.Platform.Deploy.Maintenance.Processors
{
    /// <summary>
    ///     �����������ʱ�����������
    ///     <para>* �˴������Ѿ��ϳ���</para>
    /// </summary>
    public class DSResetHeartBeatTimeRequestMessageProcessor : FunctionProcessor<DynamicServiceMessage>
    {
        #region Overrides of FunctionProcessor<DynamicServiceMessage>

        public override DynamicServiceMessage Process(Guid id, DynamicServiceMessage message)
        {
            //DynamicServiceResetHeartBeatTimeRequestMessage msg = (DynamicServiceResetHeartBeatTimeRequestMessage) message;
            //if (msg.Interval > 0)
            //{
            //    Global.DynamicService.ResetHeartBeatInterval(msg.Interval);
            //}
            //return new DynamicServiceResetHeartBeatTimeResponseMessage{ServiceName = Global.DynamicService.ServiceName, Result = true};
            return null;
        }

        #endregion
    }
}