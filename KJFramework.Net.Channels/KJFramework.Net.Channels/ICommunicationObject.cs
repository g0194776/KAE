using System;
using KJFramework.Net.Channels.Enums;
using KJFramework.Statistics;

namespace KJFramework.Net.Channels
{
    /// <summary>
    ///     通讯对象元接口，提供了相关的基本操作。
    /// </summary>
    public interface ICommunicationObject : IStatisticable<IStatistic>, IDisposable
    {
        /// <summary>
        ///     停止
        /// </summary>
        void Abort();
        /// <summary>
        ///     打开
        /// </summary>
        void Open();
        /// <summary>
        ///     关闭
        /// </summary>
        void Close();
        /// <summary>
        ///     异步打开
        /// </summary>
        /// <param name="callback">回调函数</param>
        /// <param name="state">状态</param>
        /// <returns>返回异步结果</returns>
        IAsyncResult BeginOpen(AsyncCallback callback, Object state);
        /// <summary>
        ///     异步关闭
        /// </summary>
        /// <param name="callback">回调函数</param>
        /// <param name="state">状态</param>
        /// <returns>返回异步结果</returns>
        IAsyncResult BeginClose(AsyncCallback callback, Object state);
        /// <summary>
        ///     异步打开
        /// </summary>
        void EndOpen(IAsyncResult result);
        /// <summary>
        ///     异步关闭
        /// </summary>
        void EndClose(IAsyncResult result);
        /// <summary>
        ///     获取或设置当前可用状态
        /// </summary>
        bool Enable { get; set; }
        /// <summary>
        ///     获取当前通讯状态
        /// </summary>
        CommunicationStates CommunicationState { get; }
        /// <summary>
        ///     已关闭事件
        /// </summary>
        event EventHandler Closed;
        /// <summary>
        ///     正在关闭事件
        /// </summary>
        event EventHandler Closing;
        /// <summary>
        ///     已错误事件
        /// </summary>
        event EventHandler Faulted;
        /// <summary>
        ///     已开启事件
        /// </summary>
        event EventHandler Opened;
        /// <summary>
        ///     正在开启事件
        /// </summary>
        event EventHandler Opening;
    }
}