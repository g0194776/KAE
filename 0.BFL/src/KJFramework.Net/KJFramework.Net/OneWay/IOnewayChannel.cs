using System;
using KJFramework.EventArgs;
using KJFramework.Net.ProtocolStacks;
using KJFramework.Net.Spy;

namespace KJFramework.Net.OneWay
{
    /// <summary>
    ///     单方向信道元接口，提供了相关的基本操作
    /// </summary>
    /// <typeparam name="T">消息父类类型</typeparam>
    public interface IOnewayChannel<T> : ICommunicationChannelAddress
    {
        /// <summary>
        ///     获取当前信道的连接状态
        /// </summary>
        bool Connected { get; }
        /// <summary>
        ///     获取创建时间
        /// </summary>
        DateTime CreateTime { get; }
        /// <summary>
        ///     获取通道唯一标示
        /// </summary>
        Guid Key { get; }
        /// <summary>
        ///     获取协议栈
        /// </summary>
        IProtocolStack ProtocolStack { get; }
        /// <summary>
        ///     连接到远程终结点
        /// </summary>
        /// <param name="channel">基于流的通讯信道</param>
        /// <exception cref="NullReferenceException">远程终结点地址不能为空</exception>
        void Connect(IRawTransportChannel channel);
        /// <summary>
        ///     断开当前信道的连接
        /// </summary>
        void Disconnect();
        /// <summary>
        ///     注册一个消息拦截器
        /// </summary>
        /// <param name="spy">消息拦截器</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        /// <exception cref="ArgumentException">非法参数</exception>
        void RegistSpy(IMessageSpy<T> spy);
        /// <summary>
        ///     信道已连接事件
        /// </summary>
        event EventHandler ChannelConnected;
        /// <summary>
        ///     信道已断开事件
        /// </summary>
        event EventHandler ChannelDisconnected;
        /// <summary>
        ///     已拦截未知消息事件
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<T>> UnknownSpyMessage;
    }
}