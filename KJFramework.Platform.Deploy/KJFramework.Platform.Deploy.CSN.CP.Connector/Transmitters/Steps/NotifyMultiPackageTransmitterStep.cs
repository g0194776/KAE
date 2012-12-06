using KJFramework.Logger;
using KJFramework.Net.Channels;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Enums;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Subscribers;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Transmitters.Contexts;
using KJFramework.Platform.Deploy.CSN.ProtocolStack;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector.Transmitters.Steps
{
    /// <summary>
    ///     ֪ͨ��Ҫ���еĶ�����䲽��
    /// </summary>
    public class NotifyMultiPackageTransmitterStep : TransmitteStep
    {
        #region Overrides of TransmitteStep

        /// <summary>
        ///     ִ��һ������
        /// </summary>
        /// <param name="configSubscriber">���ö�����</param>
        /// <param name="context">������</param>
        /// <param name="args">��ز���</param>
        /// <returns>����ִ�к��״̬</returns>
        protected override TransmitterSteps InnerDo(IConfigSubscriber configSubscriber, ITransmitterContext context, params object[] args)
        {
            CSNMultiPackageDataNotifyRequestMessage requestMessage = new CSNMultiPackageDataNotifyRequestMessage();
            requestMessage.PreviousSessionId = context.PreviousSessionId;
            requestMessage.SerialNumber = context.TaskId;
            ITransportChannel transportChannel = Global.ClientNode.GetTransportChannel(configSubscriber.ChannelId);
            if (transportChannel == null)
            {
                Logs.Logger.Log("#Can not get a transport channel. id: " + configSubscriber.ChannelId);
                return TransmitterSteps.Exception;
            }
            requestMessage.Bind();
            transportChannel.Send(requestMessage.Body);
            return TransmitterSteps.BeginTransfer;
        }

        #endregion
    }
}