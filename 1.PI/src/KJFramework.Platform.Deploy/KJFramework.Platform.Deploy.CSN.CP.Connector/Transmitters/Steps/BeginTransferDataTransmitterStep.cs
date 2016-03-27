using KJFramework.Logger;
using KJFramework.Net.Channels;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Enums;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Subscribers;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Transmitters.Contexts;
using KJFramework.Platform.Deploy.CSN.ProtocolStack;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector.Transmitters.Steps
{
    /// <summary>
    ///     开始传输数据标示消息报的传输步骤
    /// </summary>
    public class BeginTransferDataTransmitterStep : TransmitteStep
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