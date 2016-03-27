using KJFramework.Net.Transaction.Agent;
using KJFramework.Net.Transaction.Objects;
using KJFramework.Net.Transaction.Processors;
using System;

namespace KJFramework.Net.Transaction.Schedulers
{
    /// <summary>
    ///     请求分发器元接口，提供了相关的基本操作
    /// </summary>
    public interface IRequestScheduler<TMessage>
    {
        /// <summary>
        ///     注册一个连接代理器
        /// </summary>
        /// <param name="agent">连接代理器</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        void Regist(IServerConnectionAgent<TMessage> agent);
        /// <summary>
        ///     注册一个消息处理器
        /// </summary>
        /// <param name="protocol">消息处理协议</param>
        /// <param name="processor">处理器</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        IRequestScheduler<TMessage> Regist(Protocols protocol, IMessageProcessor processor);
    }
}