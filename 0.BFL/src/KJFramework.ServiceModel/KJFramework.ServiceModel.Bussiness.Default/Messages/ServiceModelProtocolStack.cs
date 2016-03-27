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
    ///     CONNECT.���Э��ջ
    /// </summary>
    public class ServiceModelProtocolStack : ProtocolStack
    {
        #region Constructor

        /// <summary>
        ///     CONNECT.���Э��ջ
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
        /// ��ʼ��
        /// </summary>
        /// <returns>
        /// ���س�ʼ���Ľ��
        /// </returns>
        /// <exception cref="T:KJFramework.Net.Exception.InitializeFailedException">��ʼ��ʧ��</exception>
        public override bool Initialize()
        {
            return true;
        }

        /// <summary>
        /// ����Ԫ����
        /// </summary>
        /// <param name="data">Ԫ����</param>
        /// <returns>
        /// �����ܷ������һ����ʾ
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
        ///     ����Ԫ����
        /// </summary>
        /// <param name="data">��BUFF����</param>
        /// <param name="offset">����ƫ����</param>
        /// <param name="count">���ó���</param>
        /// <returns>�����ܷ������һ����ʾ</returns>
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
        ///     ��һ����Ϣת��Ϊ2������ʽ
        /// </summary>
        /// <param name="message">��Ҫת������Ϣ</param>
        /// <returns>
        ///     ����ת�����2����
        /// </returns>
        public override byte[] ConvertToBytes(object msg)
        {
            Message message = (Message) msg;
            message.Bind();
            return message.IsBind ? message.Body : null;
        }

        /// <summary>
        ///     �Զ�����ʼ����
        /// </summary>
        /// <returns>����Э��ջʵ��</returns>
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