using KJFramework.Logger;
using KJFramework.Net.Channels;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Enums;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Subscribers;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Transmitters.Contexts;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector.Transmitters.Steps
{
    /// <summary>
    ///     ���ְ��������ݲ���
    /// </summary>
    public class TransferDataWithoutMultiPackageTransmitterStep : TransmitteStep
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
            transportChannel.Send(context.ResponseMessage.Body);
            return TransmitterSteps.Finish;
        }

        #endregion
    }
}