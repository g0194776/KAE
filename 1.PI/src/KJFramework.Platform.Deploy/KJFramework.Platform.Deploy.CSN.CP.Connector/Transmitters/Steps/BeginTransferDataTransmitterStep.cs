using KJFramework.Logger;
using KJFramework.Net.Channels;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Enums;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Subscribers;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Transmitters.Contexts;
using KJFramework.Platform.Deploy.CSN.ProtocolStack;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector.Transmitters.Steps
{
    /// <summary>
    ///     ��ʼ�������ݱ�ʾ��Ϣ���Ĵ��䲽��
    /// </summary>
    public class BeginTransferDataTransmitterStep : TransmitteStep
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
            CSNBeginTransferDataRequestMessage requestMessage = new CSNBeginTransferDataRequestMessage();
            requestMessage.ConfigType = context.ConfigType;
            requestMessage.PreviousSessionId = context.PreviousSessionId;
            requestMessage.SerialNumber = context.TaskId;
            requestMessage.TotalDataLength = context.TotalDataLength;
            requestMessage.TotalPackageCount = context.TotalPackageCount;
            requestMessage.Bind();
            ITransportChannel transportChannel = Global.ClientNode.GetTransportChannel(configSubscriber.ChannelId);
            if (transportChannel == null)
            {
                Logs.Logger.Log("#Can not get a transport channel. id: " + configSubscriber.ChannelId);
                return TransmitterSteps.Exception;
            }
            transportChannel.Send(requestMessage.Body);
            return TransmitterSteps.TransferData;
        }

        #endregion
    }
}