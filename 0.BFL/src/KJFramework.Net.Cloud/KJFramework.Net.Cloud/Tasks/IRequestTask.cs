using System;
using KJFramework.EventArgs;
using KJFramework.Net.Cloud.Nodes;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Net.Channels;
using KJFramework.Tasks;

namespace KJFramework.Net.Cloud.Tasks
{
    /// <summary>
    ///     请求任务元接口，提供了相关的基本操作。
    /// </summary>
    /// <typeparam name="T">协议栈中父类消息类型。</typeparam>
    public interface IRequestTask<T> : ITask
    {
        /// <summary>
        ///     获取或设置消息
        /// </summary>
        T Message { get; set; }
        /// <summary>
        ///     获取执行结果
        /// </summary>
        T ResultMessage { get; }
        /// <summary>
        ///     获取或设置一个值，该值标示了当前任务是否已经被出租
        /// </summary>
        bool HasRented { get; set; }
        /// <summary>
        ///     获取一个值，该值标示了当前的任务是否已经超时
        /// </summary>
        bool IsTimeout { get; }
        /// <summary>
        ///     获取或设置对应的传输通道
        /// </summary>
        IMessageTransportChannel<T> Channel { get; set; }
        /// <summary>
        ///     获取任务唯一标示
        /// </summary>
        Guid TaskId { get; }
        /// <summary>
        ///     获取或设置对应的网络节点
        /// </summary>
        NetworkNode<T> Node { get; set; }
        /// <summary>
        ///     获取或设置功能处理器
        /// </summary>
        IFunctionProcessor<T> Processor { get; set; }
        /// <summary>
        ///     重置当前任务的所有状态
        /// </summary>
        void Reset();
        /// <summary>
        ///     处理超时事件
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<T>> ExecuteTimeout;
    }
}