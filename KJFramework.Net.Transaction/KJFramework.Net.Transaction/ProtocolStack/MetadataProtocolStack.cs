using KJFramework.IO.Helper;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Engine;
using KJFramework.Net.ProtocolStacks;
using KJFramework.Net.Transaction.Objects;
using KJFramework.Tracing;
using System;
using System.Collections.Generic;

namespace KJFramework.Net.Transaction.ProtocolStack
{
    /// <summary>
    ///     服务器端消息协议栈抽象父类
    /// </summary>
    public class MetadataProtocolStack : ProtocolStack<MetadataContainer>
    {
        #region Constructor

        /// <summary>
        ///     服务器端消息协议栈抽象父类
        ///     <para>* 此构造将会自动初始化当前协议栈</para>
        /// </summary>
        public MetadataProtocolStack()
        {
            Initialize();
        }

        #endregion

        #region Members

        protected static ITracing _tracing = TracingManager.GetTracing(typeof(MetadataProtocolStack));

        #endregion

        #region Overrides of ProtocolStack<MetadataContainer>

        /// <summary>
        ///    初始化
        /// </summary>
        /// <returns>返回初始化的结果</returns>
        public override bool Initialize()
        {
            return true;
        }

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
            List<MetadataContainer> messages = new List<MetadataContainer>();
            try
            {
                while (count > 0)
                {
                    totalLength = BitConverter.ToInt32(data, offset);
                    if (totalLength > data.Length)
                    {
                        _tracing.Error("#Parse message failed, illegal total length. #length: " + totalLength);
                        return messages;
                    }
                    int markRangeCount = BitConverter.ToUInt16(data, offset + 4);
                    int markRangeLength = markRangeCount*5 + offset;
                    int protocolId = data[markRangeLength + 6];
                    int serviceId = data[markRangeLength + 7];
                    int detailsId = data[markRangeLength + 8];
                    MetadataContainer message;
                    try
                    {
                        message = MetadataObjectEngine.GetObject(data, (uint) offset, (uint) totalLength);
                        if (message == null)
                        {
                            _tracing.Error(
                                "#Parse message failed, parse result = null. #protocol id={0}, service id={1}, detalis id={2}: ",
                                protocolId, serviceId, detailsId);
                            return messages;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        _tracing.Error(ex, "#Parse message failed.");
                        continue;
                    }
                    finally
                    {
                        offset += totalLength;
                        count -= totalLength;
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