using System;
using System.IO;
using KJFramework.Statistics;

namespace KJFramework.Net.Channels.Transactions
{
    /// <summary>
    ///     流事物元接口，提供了相关的基本操作。
    /// </summary>
    internal interface IStreamTransaction<TStream> : IDisposable, IStatisticable<IStatistic>
        where TStream : Stream
    {
        /// <summary>
        ///     获取或设置一个值，该值表示了当前事物是否可以异步执行
        /// </summary>
        bool CanAsync { get; set; }
        /// <summary>
        ///     获取一个值，该值标示了当前流事物的状态
        /// </summary>
        bool Enable { get; }
        /// <summary>
        ///     获取内部流
        /// </summary>
        TStream Stream { get; }
        /// <summary>
        ///     注册回调
        /// </summary>
        /// <param name="action">回调</param>
        StreamTransaction<TStream> RegistCallback(Action<byte[]> action);
        /// <summary>
        ///     停止工作事件
        /// </summary>
        event EventHandler Disconnected;
    }
}
