using KJFramework.Logger;
using KJFramework.Net.Channels;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Enums;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Subscribers;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Transmitters.Contexts;
using KJFramework.Platform.Deploy.CSN.ProtocolStack;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector.Transmitters.Steps
{
    /// <summary>
    ///     分包传输数据步骤
    /// </summary>
    public class TransferDataTransmitterStep : TransmitteStep
    {
        #region Overrides of TransmitteStep

        /// <summary>
        ///     执行一个操作
        /// </summary>
        /// <param name="configSubscriber">配置订阅者</param>
        /// <param name="context">上下文</param>
        /// <param name="args">相关参数</param>
        /// <returns>返回执行后的状态</returns>
        protected override TransmitterSteps InnerDo(IConfigSubscriber configSubscriber, ITransmitterContext context, params object[] args)
        {
            ITransportChannel transportChannel = Global.ClientNode.GetTransportChannel(configSubscriber.ChannelId);
            if (transportChannel == null)
            {
                Logs.Logger.Log("#Can not get a transport channel. id: " + configSubscriber.ChannelId);
                return TransmitterSteps.Exception;
            }
            //全包发送
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
            //丢包补偿
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