using KJFramework.IO.Helper;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Engine;
using KJFramework.Messages.ValueStored.DataProcessor.Mapping;
using KJFramework.Net.ProtocolStacks;
using KJFramework.Net.Transaction.Objects;
using KJFramework.Net.Transaction.ValueStored;
using KJFramework.Tracing;
using System;
using System.Collections.Generic;

namespace KJFramework.Net.Transaction.ProtocolStack
{
    /// <summary>
    ///     服务器端消息协议栈抽象父类
    /// </summary>
    public abstract class MetadataProtocolStack : ProtocolStack<MetadataContainer>
    {
        #region Constructor

        /// <summary>
        ///     服务器端消息协议栈抽象父类
        ///     <para>* 此构造将会自动初始化当前协议栈</para>
        /// </summary>
        protected MetadataProtocolStack()
        {
        }

        #endregion

        #region Members

        protected static ITracing _tracing = TracingManager.GetTracing(typeof(MetadataProtocolStack));

        #endregion

        #region Overrides of ProtocolStack<MetadataContainer>

        /// <summary>
        /// 解析元数据
        /// </summary>
        /// <param name="data">元数据</param>
        /// <returns>
        /// 返回能否解析的一个标示
        /// </returns>
        public override List<MetadataContainer> Parse(byte[] data)
        {
            return Parse(data, 0, data.Length);
        }

        /// <summary>
        /// 将一个消息转换为2进制形式
        /// </summary>
        /// <param name="message">需要转换的消息</param>
        /// <returns>
        /// 返回转换后的2进制
        /// </returns>
        public override byte[] ConvertToBytes(MetadataContainer message)
        {
            return MetadataObjectEngine.ToBytes(message);
        }

        protected Type GetMessageType(int protocolId, int serviceId, int detailsId)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     获取一个协议站内所有的消息定义
        /// </summary>
        /// <returns>返回消息定义集合</returns>
        internal Dictionary<Protocols, Type> GetMessageDefinitions()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     解析元数据
        /// </summary>
        /// <param name="data">总BUFF长度</param>
        /// <param name="offset">可用偏移量</param>
        /// <param name="count">可用长度</param>
        /// <returns>
        ///     返回能否解析的一个标示
        /// </returns>
        public override List<MetadataContainer> Parse(byte[] data, int offset, int count)
        {
            int totalLength;
            ExtensionTypeMapping.Regist(typeof(MessageIdentityValueStored));
            List<MetadataContainer> messages = new List<MetadataContainer>();
            try
            {
                while (offset < data.Length)
                {
                    totalLength = BitConverter.ToInt32(data, offset);
                    if (totalLength > data.Length)
                    {
                        _tracing.Error("#Parse message failed, illegal total length. #length: " + totalLength);
                        return messages;
                    }
                    byte[] messageData = totalLength == data.Length
                                             ? data
                                             : ByteArrayHelper.GetReallyData(data, offset, totalLength);
                    int markRangeCount = BitConverter.ToUInt16(data, offset + 4);
                    int markRangeLength = markRangeCount * 5;
                    int protocolId = messageData[markRangeLength + 6];
                    int serviceId = messageData[markRangeLength + 7];
                    int detailsId = messageData[markRangeLength + 8];
                    offset += messageData.Length;
                    MetadataContainer message;
                    try
                    {
                        message = MetadataObjectEngine.GetObject(messageData, 0, (uint)messageData.Length);
                        if (message == null)
                        {
                            _tracing.Error("#Parse message failed, parse result = null. #protocol id={0}, service id={1}, detalis id={2}: ", protocolId, serviceId, detailsId);
                            return messages;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        _tracing.Error(ex, "#Parse message failed.");
                        continue;
                    }
                    messages.Add(message);
                    if (data.Length - offset < 4) break;
                }
                return messages;
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, "#Parse message failed.");
                return messages;
            }
        }

        /// <summary>
        ///     将当前协议栈与指定协议栈进行合并
        /// </summary>
        /// <param name="protocolStack">要合并的协议栈</param>
        /// <returns>返回合并后的当前协议栈</returns>
        public MetadataProtocolStack Combine(MetadataProtocolStack protocolStack)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}