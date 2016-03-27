using System;
using KJFramework.EventArgs;
using KJFramework.Net.Cloud.Accessors;
using KJFramework.Net.Cloud.Accessors.Rules;
using KJFramework.Net.Cloud.Exceptions;
using KJFramework.Net.Cloud.Objects;
using KJFramework.Net.ProtocolStacks;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.HostChannels;

namespace KJFramework.Net.Cloud.Nodes
{
    /// <summary>
    ///     网络节点元接口，提供了相关的基本操作。
    /// </summary>
    /// <typeparam name="T">协议栈中父类消息类型。</typeparam>
    public interface INetworkNode<T> : INetServiceNode
    {
        /// <summary>
        ///     获取或设置访问器
        /// </summary>
        IAccessor Accessor { get; set; }
        /// <summary>
        ///     获取唯一键值
        /// </summary>
        Guid Id { get; }
        /// <summary>
        ///     获取协议栈
        /// </summary>
        IProtocolStack<T> ProtocolStack { get; }
        /// <summary>
        ///     将指定元数据广播到所有当前所有的信道上。
        /// </summary>
        /// <param name="data">需要广播的元数据</param>
        void Broadcast(byte[] data);
        /// <summary>
        ///   将指定消息广播到所有当前所有的信道上。 
        /// </summary>
        /// <param name="message">需要广播的消息</param>
        void Broadcast(T message);
        /// <summary>
        ///     开启当前网络节点
        /// </summary>
        void Open();
        /// <summary>
        ///     关闭当前网络节点
        /// </summary>
        void Close();
        /// <summary>
        ///     从一个传输通道中，执行连接到远程终结点的操作
        /// </summary>
        /// <param name="channel">传输通道</param>
        /// <returns>返回连接的状态</returns>
        bool Connect(IRawTransportChannel channel);
        /// <summary>
        ///     获取一个具有指定ID的传输通道
        /// </summary>
        /// <param name="id">通道唯一标示</param>
        /// <returns>返回传输通道</returns>
        ITransportChannel GetTransportChannel(Guid id);
        /// <summary>
        ///     注册一个宿主通道
        /// </summary>
        /// <param name="channel">宿主通道</param>
        /// <exception cref="System.ArgumentNullException">参数错误</exception>
        /// <exception cref="System.Exception">注册失败</exception>
        void Regist(IHostTransportChannel channel);
        /// <summary>
        ///     注销一个宿主通道
        /// </summary>
        /// <param name="id">宿主通道唯一标示</param>
        void UnRegist(Guid id);
        /// <summary>
        ///     发送元数据到具有指定通道编号的远程节点上
        /// </summary>
        /// <param name="id">传输通道编号</param>
        /// <param name="data">元数据</param>
        /// <exception cref="TransportChannelNotFoundException">传输通道不存在</exception>
        void Send(Guid id, byte[] data);
        /// <summary>
        ///     发送一个消息到具有指定通道编号的远程节点上
        /// </summary>
        /// <param name="id">传输通道编号</param>
        /// <param name="message">元数据</param>
        /// <exception cref="TransportChannelNotFoundException">传输通道不存在</exception>
        /// <exception cref="System.ArgumentNullException">参数错误</exception>
        void Send(Guid id, T message);
        /// <summary>
        ///     被拒绝的链接事件
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<IAccessRule>> ConnectedButNotAllow;
        /// <summary>
        ///     创建新的传输通道事件
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<IRawTransportChannel>> NewTransportChannelCreated;
        /// <summary>
        ///     接收到新消息事件
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<ReceivedMessageObject<T>>> NewMessageReceived;
        /// <summary>
        ///     传输通道被移除事件
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<Guid>> TransportChannelRemoved;
    }
}