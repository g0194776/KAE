using KJFramework.IO.Helper;
using KJFramework.Platform.Deploy.CSN.Common.Configurations;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Enums;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Objects;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Subscribers;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Transmitters.Contexts;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector.Transmitters.Steps
{
    /// <summary>
    ///     ��ʼ�����ԵĴ��䲽��
    /// </summary>
    public class InitializePolicyTransmitterStep : TransmitteStep
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
            byte[] data = context.ResponseMessage.Body;
            int length = data.Length;
            //���÷ְ��߼�
            if (length > CSNSettingConfigSection.Current.Settings.MaxDataChunkSize)
            {
                int offset = 0;
                int last;
                int currentSize;
                int size = length / CSNSettingConfigSection.Current.Settings.MaxDataChunkSize + (length == CSNSettingConfigSection.Current.Settings.MaxDataChunkSize ? 0 : 1);
                DataPart[] messages = new DataPart[size];
                context.TotalPackageCount = size;
                context.TotalDataLength = length;
                for (int i = 0; i < size; i++)
                {
                    last = length - offset;
                    byte[] temp = ByteArrayHelper.GetNextData(data, offset, (currentSize = (last > CSNSettingConfigSection.Current.Settings.MaxDataChunkSize ? CSNSettingConfigSection.Current.Settings.MaxDataChunkSize : last)));
                    offset += currentSize;
                    DataPart dataPart = new DataPart();
                    dataPart.Data = temp;
                    messages[i] = dataPart;
                }
                return TransmitterSteps.Notify;
            }
            //ֱ������
            return TransmitterSteps.TransferDataWithoutMultiPackage;
        }

        #endregion
    }
}