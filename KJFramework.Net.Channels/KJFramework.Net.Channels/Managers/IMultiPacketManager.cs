using System;
using KJFramework.EventArgs;
using KJFramework.Net.Channels.Caches;

namespace KJFramework.Net.Channels.Managers
{
    /// <summary>
    ///     封包片管理器元接口，提供了相关的基本操作
    /// </summary>
    /// <typeparam name="T">消息类型</typeparam>
    public interface IMultiPacketManager<T> : IDisposable
    {
        /// <summary>
        ///     添加一个封包片
        /// </summary>
        /// <param name="key">唯一消息Id</param>
        /// <param name="message">封包片</param>
        /// <param name="maxPacketCount">
        ///     最大封包片数
        ///     <para>* 第一次调用时设置此值，以后默认传-1即可。</para>
        /// </param>
        /// <returns>如果返回值不为null, 则证明已经拼接为一个完整的消息</returns>
        T Add(int key, T message, int maxPacketCount = -1);
        /// <summary>
        ///     添加一个封包片
        /// </summary>
        /// <param name="key">唯一消息Id</param>
        /// <param name="message">封包片</param>
        /// <param name="timeSpan">过期时间</param>
        /// <param name="maxPacketCount">
        ///     最大封包片数
        ///     <para>* 第一次调用时设置此值，以后默认传-1即可。</para>
        /// </param>
        /// <returns>如果返回值不为null, 则证明已经拼接为一个完整的消息</returns>
        T Add(int key, T message, TimeSpan timeSpan, int maxPacketCount = -1);
        /// <summary>
        ///     封包消息过期事件
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<IMultiPacketStub<T>>> Expired;
    }
}