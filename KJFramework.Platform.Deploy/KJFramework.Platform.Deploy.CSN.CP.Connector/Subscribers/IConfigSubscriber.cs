using System;
using KJFramework.Platform.Deploy.CSN.CP.Connector.SubscribeObjs;
using KJFramework.Platform.Deploy.CSN.ProtocolStack;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector.Subscribers
{
    /// <summary>
    ///     配置订约人元接口，提供了相关的基本操作。
    /// </summary>
    public interface IConfigSubscriber
    {
        /// <summary>
        ///     获取或设置一个值，该值标示了当前用户是否需要动态反向刷新配置的能力
        /// </summary>
        bool NeedUpdate { get; set; }
        /// <summary>
        ///     获取或设置订约人的关键序列值
        /// </summary>
        string SubscriberKey { get; }
        /// <summary>
        ///     获取或设置预订约人相关的网络通道编号
        /// </summary>
        Guid ChannelId { get; set; }
        /// <summary>
        ///     向配置订阅者发送消息
        /// </summary>
        /// <param name="message">发送的消息</param>
        void Send(CSNMessage message);
        /// <summary>
        ///     取消当前订阅者的所有订阅信息
        /// </summary>
        void Cancel();
        /// <summary>
        ///     获取订阅对象
        /// </summary>
        /// <typeparam name="T">订阅对象类型</typeparam>
        /// <returns>返回订阅对象</returns>
        T GetSubscribeObject<T>() where T : ISubscribeObject;
        /// <summary>
        ///     订阅已取消事件
        /// </summary>
        event EventHandler SubscribeCanceled;
    }
}