using System;
using System.Collections.Generic;
using KJFramework.Net.Cloud.Exceptions;
using KJFramework.Net.Cloud.Processors;

namespace KJFramework.Net.Cloud.Nodes
{
    /// <summary>
    ///     基于消息驱动机制的功能节点
    /// </summary>
    /// <typeparam name="T">协议栈中父类消息类型。</typeparam>
    public abstract class MessageFunctionNode<T> : FunctionNode<T>, IMessageFunctionNode<T>
    {
        #region Members

        protected Dictionary<Guid, IFunctionProcessor<T>> _processors = new Dictionary<Guid, IFunctionProcessor<T>>();
        protected Dictionary<Type, IFunctionProcessor<T>> _cacheProcessor = new Dictionary<Type, IFunctionProcessor<T>>();

        /// <summary>
        ///     获取处理器个数
        /// </summary>
        public int ProcessorCount
        {
            get { return _processors.Count; }
        }

        #endregion

        #region Functions

        /// <summary>
        ///     探测一个消息是否能被当前处理器所处理
        /// </summary>
        /// <param name="id">传输通道标示</param>
        /// <param name="message">探测的消息</param>
        /// <returns>返回是否能被处理的一个标示</returns>
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
        ///     检测指定消息是否能处理
        /// </summary>
        /// <param name="message">被检测的消息</param>
        /// <returns>
        ///     返回检测后的消息
        ///     <para>* 如果可以处理，请返回能处理该消息的功能处理器。</para>
        ///     <para>* 如果不能处理，请返回null。</para>
        /// </returns>
        protected abstract IFunctionProcessor<T> CheckMessageCanProcessed(T message);

        /// <summary>
        ///     获取具有指定标示的功能处理器
        /// </summary>
        /// <param name="id">功能处理器标示</param>
        /// <returns>返回功能处理器</returns>
        public virtual IFunctionProcessor<T> GetProcessor(Guid id)
        {
            return _processors.ContainsKey(id) ? _processors[id] : null;
        }

        /// <summary>
        ///     处理一个消息
        /// </summary>
        /// <param name="id">传输通道标示</param>
        /// <param name="message">要处理的消息</param>
        /// <returns>
        ///     返回反馈消息
        ///     <para>* 如果返回null, 则认为没有反馈消息。</para>
        /// </returns>
        /// <exception cref="NotSupportedProcessException">不支持的操作</exception>
        /// <exception cref="FunctionProcessorNotEnableException">功能处理器未启动</exception>
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