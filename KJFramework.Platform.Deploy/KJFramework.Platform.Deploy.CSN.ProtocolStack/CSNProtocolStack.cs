using System;
using System.Collections.Generic;
using KJFramework.Basic.Enum;
using KJFramework.EventArgs;
using KJFramework.IO.Helper;
using KJFramework.Logger;
using KJFramework.Messages.Engine;
using KJFramework.Platform.Deploy.CSN.ProtocolStack.Statistics;
using KJFramework.Statistics;

namespace KJFramework.Platform.Deploy.CSN.ProtocolStack
{
    /// <summary>
    ///     ����վ�ڵ�Э��ջ���ṩ����صĻ���������
    /// </summary>
    public class CSNProtocolStack : Net.ProtocolStacks.ProtocolStack<CSNMessage>, IStatisticable<IStatistic>
    {
        #region Constructor

        /// <summary>
        ///     ��̬����Э��ջ���ṩ����صĻ���������
        /// </summary>
        public CSNProtocolStack()
        {
            CSNProtocolStackStatistic statistic = new CSNProtocolStackStatistic();
            statistic.Initialize(this);
            _statistics.Add(StatisticTypes.Other, statistic);
        }

        ~CSNProtocolStack()
        {
            if (_statistics != null)
            {
                foreach (KeyValuePair<StatisticTypes, IStatistic> keyValuePair in _statistics)
                {
                    keyValuePair.Value.Close();
                }
                _statistics.Clear();
                _statistics = null;
            }
        }

        #endregion

        #region Members

        private Dictionary<StatisticTypes, IStatistic> _statistics = new Dictionary<StatisticTypes, IStatistic>();
        private Dictionary<int, Type> _messageTypes = new Dictionary<int, Type>();

        #endregion

        #region Overrides of ProtocolStack<DynamicServiceMessage>

        /// <summary>
        ///     ��ʼ��
        /// </summary>
        /// <returns>
        ///     ���س�ʼ���Ľ��
        /// </returns>
        /// <exception cref="T:KJFramework.Net.Exception.InitializeFailedException">��ʼ��ʧ��</exception>
        public override bool Initialize()
        {
            _messageTypes.Add(0, typeof(CSNRegistRequestMessage));
            _messageTypes.Add(1, typeof(CSNRegistResponseMessage));
            _messageTypes.Add(2, typeof(CSNGetDataTableRequestMessage));
            _messageTypes.Add(3, typeof(CSNGetDataTableResponseMessage));
            _messageTypes.Add(4, typeof(CSNGetKeyDataRequestMessage));
            _messageTypes.Add(5, typeof(CSNGetKeyDataResponseMessage));
            _messageTypes.Add(6, typeof(CSNMultiPackageDataNotifyRequestMessage));
            _messageTypes.Add(7, typeof(CSNMultiPackageDataNotifyResponseMessage));
            _messageTypes.Add(8, typeof(CSNBeginTransferDataRequestMessage));
            _messageTypes.Add(9, typeof(CSNBeginTransferDataResponseMessage));
            _messageTypes.Add(10, typeof(CSNTransferDataMessage));
            _messageTypes.Add(11, typeof(CSNEndTransferDataRequestMessage));
            //_messageTypes.Add(8, typeof(DSNDeployStatusReportMessage));
            //_messageTypes.Add(9, typeof(DSNUnDeployServiceRequestMessage));
            //_messageTypes.Add(10, typeof(DSNUnDeployServiceResponseMessage));
            //_messageTypes.Add(11, typeof(DSNStartServiceRequestMessage));
            //_messageTypes.Add(12, typeof(DSNStartServiceResponseMessage));
            //_messageTypes.Add(13, typeof(DSNStopServiceRequestMessage));
            //_messageTypes.Add(14, typeof(DSNStopServiceResponseMessage));
            //_messageTypes.Add(15, typeof(DSNGetLocalServiceInfomationRequestMessage));
            //_messageTypes.Add(16, typeof(DSNGetLocalServiceInfomationResponseMessage)););
            return true;
        }

        /// <summary>
        /// ����Ԫ����
        /// </summary>
        /// <param name="data">Ԫ����</param>
        /// <returns>
        /// �����ܷ������һ����ʾ
        /// </returns>
        public override List<CSNMessage> Parse(byte[] data)
        {
            int offset = 0;
            int totalLength;
            List<CSNMessage> messages = new List<CSNMessage>();
            try
            {
                while (offset < data.Length)
                {
                    totalLength = BitConverter.ToInt32(ByteArrayHelper.GetReallyData(data, offset, 4), 0);
                    if (totalLength > data.Length)
                    {
                        ParseMessageFailedHandler(new LightSingleArgEventArgs<byte[]>(data));
                        return messages;
                    }
                    byte[] messageData = ByteArrayHelper.GetReallyData(data, offset, totalLength);
                    int protocolId = BitConverter.ToInt32(ByteArrayHelper.GetNextData(messageData, 18, 4), 0);
                    Type messageType = GetMessageType(protocolId);
                    if (messageType == null)
                    {
                        ParseMessageFailedHandler(new LightSingleArgEventArgs<byte[]>(messageData));
                        return messages;
                    }
                    offset += messageData.Length;
                    CSNMessage message;
                    try
                    {
                        //ʹ�����ܶ���������н���
                        message = IntellectObjectEngine.GetObject<CSNMessage>(messageType, messageData);
                        if (message == null)
                        {
                            ParseMessageFailedHandler(new LightSingleArgEventArgs<byte[]>(messageData));
                            return messages;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        ParseMessageFailedHandler(new LightSingleArgEventArgs<byte[]>(messageData));
                        Logs.Logger.Log(ex);
                        continue;
                    }
                    ParseMessageSuccessedHandler(new LightSingleArgEventArgs<CSNMessage>(message));
                    messages.Add(message);
                    if (data.Length - offset < 4)
                    {
                        break;
                    }
                }
                return messages;
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
                return messages;
            }
        }

        /// <summary>
        /// ��һ����Ϣת��Ϊ2������ʽ
        /// </summary>
        /// <param name="message">��Ҫת������Ϣ</param>
        /// <returns>
        /// ����ת�����2����
        /// </returns>
        public override byte[] ConvertToBytes(CSNMessage message)
        {
            if (message == null)
            {
                return null;
            }
            message.Bind();
            return message.IsBind ? message.Body : null;
        }

        #endregion

        #region Implementation of IStatisticable<IStatistic>

        /// <summary>
        /// ��ȡ������ͳ����
        /// </summary>
        public Dictionary<StatisticTypes, IStatistic> Statistics
        {
            get { return _statistics; }
            set { _statistics = value; }
        }

        #endregion

        #region Events

        /// <summary>
        ///     ������Ϣʧ��
        /// </summary>
        internal event EventHandler<LightSingleArgEventArgs<byte[]>> ParseMessageFailed;
        protected void ParseMessageFailedHandler(LightSingleArgEventArgs<byte[]> e)
        {
            EventHandler<LightSingleArgEventArgs<byte[]>> handler = ParseMessageFailed;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        ///     ������Ϣ�ɹ�
        /// </summary>
        internal event EventHandler<LightSingleArgEventArgs<CSNMessage>> ParseMessageSuccessed;
        protected void ParseMessageSuccessedHandler(LightSingleArgEventArgs<CSNMessage> e)
        {
            EventHandler<LightSingleArgEventArgs<CSNMessage>> handler = ParseMessageSuccessed;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        ///     ת��Ԫ����ʧ��
        /// </summary>
        internal event EventHandler<LightSingleArgEventArgs<CSNMessage>> MessageToByteFailed;
        protected void MessageToByteFailedHandler(LightSingleArgEventArgs<CSNMessage> e)
        {
            EventHandler<LightSingleArgEventArgs<CSNMessage>> handler = MessageToByteFailed;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        ///     ת��Ԫ���ݳɹ�
        /// </summary>
        internal event EventHandler<LightSingleArgEventArgs<CSNMessage>> MessageToByteSuccessed;
        protected void MessageToByteSuccessedHandler(LightSingleArgEventArgs<CSNMessage> e)
        {
            EventHandler<LightSingleArgEventArgs<CSNMessage>> handler = MessageToByteSuccessed;
            if (handler != null) handler(this, e);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     �����ù�һ��Э���Ż�ȡָ������Ϣ����
        /// </summary>
        /// <param name="protocolId">Э����</param>
        /// <returns>������Ϣ����</returns>
        private Type GetMessageType(int protocolId)
        {
            Type value;
            if (_messageTypes.TryGetValue(protocolId, out value))
            {
                return value;
            }
            return null;
        }

        #endregion
    }
}