using System;
using System.Collections.Generic;
using System.Reflection;
using KJFramework.IO.Helper;
using KJFramework.Messages.Engine;
using KJFramework.Net.ProtocolStacks;
using KJFramework.Net.Transaction.Comparers;
using KJFramework.Net.Transaction.Messages;
using KJFramework.Net.Transaction.Objects;
using KJFramework.Tracing;

namespace KJFramework.Net.Transaction.ProtocolStack
{
    /// <summary>
    ///     服务器端消息协议栈抽象父类
    /// </summary>
    public abstract class BusinessProtocolStack : ProtocolStack<BaseMessage>
    {
        #region Constructor

        /// <summary>
        ///     服务器端消息协议栈抽象父类
        ///     <para>* 此构造将会自动初始化当前协议栈</para>
        /// </summary>
        public BusinessProtocolStack()
        {
            Initialize();
            AutoInitialize();
        }

        #endregion

        #region Members

        protected Dictionary<Protocols, Type> _messages = new Dictionary<Protocols, Type>(new ProtocolsComparer());
        protected static ITracing _tracing = TracingManager.GetTracing(typeof(BusinessProtocolStack));

        #endregion

        #region Overrides of ProtocolStack<BaseMessage>

        /// <summary>
        /// 解析元数据
        /// </summary>
        /// <param name="data">元数据</param>
        /// <returns>
        /// 返回能否解析的一个标示
        /// </returns>
        public override List<BaseMessage> Parse(byte[] data)
        {
            int offset = 0;
            int totalLength;
            List<BaseMessage> messages = new List<BaseMessage>();
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
                    int protocolId = messageData[5];
                    int serviceId = messageData[6];
                    int detailsId = messageData[7];
                    Type messageType = GetMessageType(protocolId, serviceId, detailsId);
                    if (messageType == null)
                    {
                        _tracing.Error("#Parse message failed, illegal message protocol. #protocol id={0}, service id={1}, detalis id={2}\r\nTarget protocol stack: {3} ", protocolId, serviceId, detailsId, this);
                        return messages;
                    }
                    offset += messageData.Length;
                    BaseMessage message;
                    try
                    {
                        //使用智能对象引擎进行解析
                        message = IntellectObjectEngine.GetObject<BaseMessage>(messageType, messageData);
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
        /// 将一个消息转换为2进制形式
        /// </summary>
        /// <param name="message">需要转换的消息</param>
        /// <returns>
        /// 返回转换后的2进制
        /// </returns>
        public override byte[] ConvertToBytes(BaseMessage message)
        {
            message.Bind();
            return message.Body;
        }

        protected Type GetMessageType(int protocolId, int serviceId, int detailsId)
        {
            Protocols protocols = new Protocols { ProtocolId = protocolId, ServiceId = serviceId, DetailsId = detailsId };
            Type msgType;
            return _messages.TryGetValue(protocols, out msgType) ? msgType : null;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     获取一个协议站内所有的消息定义
        /// </summary>
        /// <returns>返回消息定义集合</returns>
        internal Dictionary<Protocols, Type> GetMessageDefinitions()
        {
            return _messages;
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
        public override List<BaseMessage> Parse(byte[] data, int offset, int count)
        {
            if (data.Length - offset < count)
            {
                _tracing.Warn("#Cannot parse segment data, remaining count has no enough!");
                return null;
            }
            int protocolId = data[offset + 5];
            int serviceId = data[offset + 6];
            int detailsId = data[offset + 7];
            Type messageType = GetMessageType(protocolId, serviceId, detailsId);
            if (messageType == null)
            {
                _tracing.Error("#Parse message failed, illegal message protocol. #protocol id={0}, service id={1}, detalis id={2}\r\nTarget protocol stack: {3} ", protocolId, serviceId, detailsId, this);
                return null;
            }
            try
            {
                BaseMessage baseMessage = IntellectObjectEngine.GetObject<BaseMessage>(messageType, data, offset, count);
                return baseMessage != null ? new List<BaseMessage> { baseMessage } : null;
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, "#Parse message failed, illegal message protocol. #protocol id={0}, service id={1}, detalis id={2}\r\nTarget protocol stack: {3} ", protocolId, serviceId, detailsId, this);
                return null;
            }
        }

        /// <summary>
        ///     将当前协议栈与指定协议栈进行合并
        /// </summary>
        /// <param name="protocolStack">要合并的协议栈</param>
        /// <returns>返回合并后的当前协议栈</returns>
        public BusinessProtocolStack Combine(BusinessProtocolStack protocolStack)
        {
            Dictionary<Protocols, Type> messageDefinitions = protocolStack.GetMessageDefinitions();
            if(messageDefinitions != null)
            {
                foreach (KeyValuePair<Protocols, Type> definition in messageDefinitions)
                    _messages.Add(definition.Key, definition.Value);
            }
            return this;
        }

        /// <summary>
        ///     自动化初始工作
        /// </summary>
        /// <returns>返回协议栈实例</returns>
        public virtual BusinessProtocolStack AutoInitialize()
        {
            //discard old message collection.
            _messages = new Dictionary<Protocols, Type>();
            Assembly assembly = GetType().Assembly;
            foreach (Type type in assembly.GetTypes())
            {
                if(type.IsSubclassOf(typeof(BaseMessage)))
                {
                    //create instance for msg, need default ctor.
                    BaseMessage baseMsg = (BaseMessage) Activator.CreateInstance(type);
                    //add current message to protocol stack.
                    _messages.Add(
                        new Protocols
                            {
                                ProtocolId = baseMsg.MessageIdentity.ProtocolId,
                                ServiceId = baseMsg.MessageIdentity.ServiceId,
                                DetailsId = baseMsg.MessageIdentity.DetailsId
                            }, type);
                }
            }
            return this;
        }

        #endregion
    }
}