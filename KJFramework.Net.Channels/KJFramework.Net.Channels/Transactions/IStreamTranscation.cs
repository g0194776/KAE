using System;
using System.IO;

namespace KJFramework.Net.Channels.Transactions
{
    /// <summary>
    ///     流事物元接口，提供了相关的基本操作。
    /// </summary>
    internal interface IStreamTransaction<TStream>
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
        ///    结束当前事物的工作
        /// </summary>
        void EndWork();
        /// <summary>
        ///     停止工作事件
        /// </summary>
        event EventHandler Disconnected;
    }
}
