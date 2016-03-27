using System;
using KJFramework.Net.Cloud.Nodes;
using KJFramework.Net.Cloud.Objects;
using KJFramework.Net.Channels;
using KJFramework.Statistics;

namespace KJFramework.Net.Cloud.Schedulers
{
    /// <summary>
    ///     请求调度器元接口，提供了相关的基本操作。
    /// </summary>
    /// <typeparam name="T">协议栈中父类消息类型。</typeparam>
    public interface IRequestScheduler<T> : IStatisticable<IStatistic>
    {
        /// <summary>
        ///     获取唯一标示
        /// </summary>
        Guid Id { get; }
        /// <summary>
        ///   按照当前已经注册的功能节点，调度处理一个消息请求
        /// </summary>
        /// <param name="networkNode">网络节点</param>
        /// <param name="target">接收到的消息对象</param>
        void Schedule(NetworkNode<T> networkNode,  ReceivedMessageObject<T> target);
        /// <summary>
        ///     按照当前已经注册的功能节点，调度处理一个消息请求
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="networkNode">网络节点</param>
        /// <param name="channel">传输通道</param>
        void Schedule(T message, NetworkNode<T> networkNode, IMessageTransportChannel<T> channel);
        /// <summary>
        ///     注册网络节点
        /// </summary>
        /// <param name="node">网络节点</param>
        void Regist(INetworkNode<T> node);
        /// <summary>
        ///     注册功能节点
        /// </summary>
        /// <param name="node">功能节点</param>
        void Regist(IMessageFunctionNode<T> node);
        /// <summary>
        ///     注册网络节点
        /// </summary>
        /// <param name="id">唯一标示</param>
        void UnRegistNetworkNode(Guid id);
        /// <summary>
        ///     注册功能节点
        /// </summary>
        /// <param name="id">唯一标示</param>
        void UnRegistFunctionNode(Guid id);
        /// <summary>
        ///     开始调度
        /// </summary>
        void Start();
        /// <summary>
        ///     停止调度
        /// </summary>
        void Stop();

    }
}