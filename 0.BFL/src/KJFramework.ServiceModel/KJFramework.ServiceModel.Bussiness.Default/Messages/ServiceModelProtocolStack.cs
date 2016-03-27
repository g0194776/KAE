using System;
using System.Collections.Generic;
using System.Reflection;
using KJFramework.Messages.Engine;
using KJFramework.Net.Channels.Identities;
using KJFramework.Net.ProtocolStacks;
using KJFramework.Tracing;

namespace KJFramework.ServiceModel.Bussiness.Default.Messages
{
    /// <summary>
    ///     CONNECT.框架协议栈
    /// </summary>
    public class ServiceModelProtocolStack : ProtocolStack
    {
        #region Constructor

        /// <summary>
        ///     CONNECT.框架协议栈
        /// </summary>
        public ServiceModelProtocolStack()
        {
            Initialize();
            AutoInitialize();
        }

        #endregion

        #region Members

        private static Dictionary<MessageIdentity, Type> _messages;
        private static ITracing _tracing = TracingManager.GetTracing(typeof(ServiceModelProtocolStack));
        #endregion

        #region Overrides of ProtocolStack<Message>

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns>
        /// 返回初始化的结果
        /// </returns>
        /// <exception cref="T:KJFramework.Net.Exception.InitializeFailedException">初始化失败</exception>
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
        public override List<T> Parse<T>(byte[] data)
        {
            if (data == null) throw new System.Exception("Parse Data is null!");
            int readCount = 0;
            int offset = 0;
            int count = data.Length;
            List<T> retList = new List<T>();
            while (readCount < count)
            {
                int objsize;
                MessageIdentity mIdentity;
                unsafe
                {
                    fixed (byte* pData = &data[offset])
                    {
                        objsize = *(int*)pData;
                        mIdentity = new MessageIdentity();
                    }
                    Type type;
                    if (!_messages.TryGetValue(mIdentity, out type))
                    {
                        _tracing.Error("MessageIdentity is not exist,P:{0},S:{1},D:{2}", mIdentity.ProtocolId, mIdentity.ServiceId, mIdentity.DetailsId);
                        continue;
                    }
                    try
                    {
                        var msg = IntellectObjectEngine.GetObject<Message>(type, data, offset, objsize);
                        if (msg != null)
                            retList.Add((T) (object)msg);
                        offset += objsize;
                        readCount += objsize;
                    }
                    catch (System.Exception ex)
                    {
                        _tracing.Error(ex, "Parser Failed!");
                    }
                    if (count - readCount < 4) break;
                }
            }
            return retList;
        }

        /// <summary>
        ///     解析元数据
        /// </summary>
        /// <param name="data">总BUFF长度</param>
        /// <param name="offset">可用偏移量</param>
        /// <param name="count">可用长度</param>
        /// <returns>返回能否解析的一个标示</returns>
        public override List<T> Parse<T>(byte[] data, int offset, int count)
        {
            if (data == null) throw new System.Exception("Parse Data is null!");
            int readCount = 0;
            List<T> retList = new List<T>();
            while (readCount < count)
            {
                int objsize;
                MessageIdentity mIdentity;
                unsafe
                {
                    fixed (byte* pData = &data[offset])
                    {
                        objsize = *(int*)pData;
                        mIdentity = new MessageIdentity();
                    }
                    Type type;
                    if (!_messages.TryGetValue(mIdentity, out type))
                    {
                        _tracing.Error("MessageIdentity is not exist,P:{0},S:{1},D:{2}", mIdentity.ProtocolId, mIdentity.ServiceId, mIdentity.DetailsId);
                        continue;
                    }
                    try
                    {
                        var msg = IntellectObjectEngine.GetObject<Message>(type, data, offset, objsize);
                        if (msg != null)
                            retList.Add((T) (object)msg);
                        offset += objsize;
                        readCount += objsize;
                    }
                    catch (System.Exception ex)
                    {
                        _tracing.Error(ex, "Parser Failed!");
                    }
                    if (count - readCount < 4) break;
                }
            }
            return retList;
        }

        /// <summary>
        ///     将一个消息转换为2进制形式
        /// </summary>
        /// <param name="message">需要转换的消息</param>
        /// <returns>
        ///     返回转换后的2进制
        /// </returns>
        public override byte[] ConvertToBytes(object msg)
        {
            Message message = (Message) msg;
            message.Bind();
            return message.IsBind ? message.Body : null;
        }

        /// <summary>
        ///     自动化初始工作
        /// </summary>
        /// <returns>返回协议栈实例</returns>
        public virtual ServiceModelProtocolStack AutoInitialize()
        {
            //discard old message collection.
            _messages = new Dictionary<MessageIdentity, Type>();
            Assembly assembly = GetType().Assembly;
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsSubclassOf(typeof(Message)))
                {
                    //create instance for msg, need default ctor.
                    Message baseMsg = (Message)Activator.CreateInstance(type);
                    //add current message to protocol stack.
                    _messages.Add(
                        new MessageIdentity
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