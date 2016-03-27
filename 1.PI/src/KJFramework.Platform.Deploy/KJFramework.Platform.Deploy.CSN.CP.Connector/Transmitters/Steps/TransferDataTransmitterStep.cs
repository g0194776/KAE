using KJFramework.Logger;
using KJFramework.Net.Channels;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Enums;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Subscribers;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Transmitters.Contexts;
using KJFramework.Platform.Deploy.CSN.ProtocolStack;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector.Transmitters.Steps
{
    /// <summary>
    ///     �ְ��������ݲ���
    /// </summary>
    public class TransferDataTransmitterStep : TransmitteStep
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
            ITransportChannel transportChannel = Global.ClientNode.GetTransportChannel(configSubscriber.ChannelId);
            if (transportChannel == null)
            {
                Logs.Logger.Log("#Can not get a transport channel. id: " + configSubscriber.ChannelId);
                return TransmitterSteps.Exception;
            }
            //ȫ������
            if (args == null)
            {
                for (int i = 0; i < context.Datas.Length; i++)
                {
                    CSNTransferDataMessage transferDataMessage = new CSNTransferDataMessage();
                    transferDataMessage.Data = context.Datas[i].Data;
                    transferDataMessage.SerialNumber = context.TaskId;
                    transferDataMessage.PackageId = i;
                    transferDataMessage.Bind();
                    transportChannel.Send(transferDataMessage.Body);
                }
            }
            //��������
            else
            {
                int[] packageIds = (int[]) args[0];
                for (int i = 0; i < packageIds.Length; i++)
                {
                    int currentPackageId = packageIds[i];
                    CSNTransferDataMessage transferDataMessage = new CSNTransferDataMessage();
                    transferDataMessage.Data = context.Datas[currentPackageId].Data;
                    transferDataMessage.SerialNumber = context.TaskId;
                    transferDataMessage.PackageId = currentPackageId;
                    transferDataMessage.Bind();
                    transportChannel.Send(transferDataMessage.Body);
                }
            }
            return TransmitterSteps.EndTransfer;
        }

        #endregion
    }
}