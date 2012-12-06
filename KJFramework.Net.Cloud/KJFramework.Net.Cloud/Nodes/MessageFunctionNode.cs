using System;
using System.Collections.Generic;
using KJFramework.Net.Cloud.Exceptions;
using KJFramework.Net.Cloud.Processors;

namespace KJFramework.Net.Cloud.Nodes
{
    /// <summary>
    ///     ������Ϣ�������ƵĹ��ܽڵ�
    /// </summary>
    /// <typeparam name="T">Э��ջ�и�����Ϣ���͡�</typeparam>
    public abstract class MessageFunctionNode<T> : FunctionNode<T>, IMessageFunctionNode<T>
    {
        #region Members

        protected Dictionary<Guid, IFunctionProcessor<T>> _processors = new Dictionary<Guid, IFunctionProcessor<T>>();
        protected Dictionary<Type, IFunctionProcessor<T>> _cacheProcessor = new Dictionary<Type, IFunctionProcessor<T>>();

        /// <summary>
        ///     ��ȡ����������
        /// </summary>
        public int ProcessorCount
        {
            get { return _processors.Count; }
        }

        #endregion

        #region Functions

        /// <summary>
        ///     ̽��һ����Ϣ�Ƿ��ܱ���ǰ������������
        /// </summary>
        /// <param name="id">����ͨ����ʾ</param>
        /// <param name="message">̽�����Ϣ</param>
        /// <returns>�����Ƿ��ܱ������һ����ʾ</returns>
        public IFunctionProcessor<T> CanProcess(Guid id, T message)
        {
            IFunctionProcessor<T> processor = CheckMessageCanProcessed(message);
            //can not process it.
            if (processor == null)
            {
                return null;
            }
            _processors.Add(processor.Id, processor);
            return processor;
        }

        /// <summary>
        ///     ���ָ����Ϣ�Ƿ��ܴ���
        /// </summary>
        /// <param name="message">��������Ϣ</param>
        /// <returns>
        ///     ���ؼ������Ϣ
        ///     <para>* ������Դ����뷵���ܴ������Ϣ�Ĺ��ܴ�������</para>
        ///     <para>* ������ܴ����뷵��null��</para>
        /// </returns>
        protected abstract IFunctionProcessor<T> CheckMessageCanProcessed(T message);

        /// <summary>
        ///     ��ȡ����ָ����ʾ�Ĺ��ܴ�����
        /// </summary>
        /// <param name="id">���ܴ�������ʾ</param>
        /// <returns>���ع��ܴ�����</returns>
        public virtual IFunctionProcessor<T> GetProcessor(Guid id)
        {
            return _processors.ContainsKey(id) ? _processors[id] : null;
        }

        /// <summary>
        ///     ����һ����Ϣ
        /// </summary>
        /// <param name="id">����ͨ����ʾ</param>
        /// <param name="message">Ҫ�������Ϣ</param>
        /// <returns>
        ///     ���ط�����Ϣ
        ///     <para>* �������null, ����Ϊû�з�����Ϣ��</para>
        /// </returns>
        /// <exception cref="NotSupportedProcessException">��֧�ֵĲ���</exception>
        /// <exception cref="FunctionProcessorNotEnableException">���ܴ�����δ����</exception>
        public T Process(Guid id, T message)
        {
            //at here, the basic framework can make sure message not null.
            if (_enable)
            {
                Type msgType = message.GetType();
                if (_cacheProcessor.ContainsKey(msgType))
                {
                    return _cacheProcessor[msgType].Process(id, message);
                }
                IFunctionProcessor<T> processor;
                if ((processor = CanProcess(id, message)) == null)
                {
                    throw new NotSupportedProcessException();
                }
                return processor.Process(id, message);
            }
            throw new FunctionProcessorNotEnableException();
        }

        #endregion
    }
}