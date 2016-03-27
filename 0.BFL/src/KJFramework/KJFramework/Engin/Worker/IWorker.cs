using System;
using KJFramework.EventArgs;

namespace KJFramework.Engin.Worker
{
    /// <summary>
    ///     工作者元接口，提供了相关的基本操作。
    /// </summary>
    /// <typeparam name="T">宿主类型</typeparam>
    public interface IWorker<T> : IDisposable
    {
        /// <summary>
        ///     获取当前工作者的工作状态
        /// </summary>
        bool State { get; }
        /// <summary>
        ///     执行方法，使用当前线程进行方法的执行操作。
        /// </summary>
        /// <param name="item">工作宿主 [ref]</param>
        /// <returns>返回工作的状态</returns>
        bool DoWork(T item);
        /// <summary>
        ///     执行方法，异步进行方法的执行操作。
        /// </summary>
        /// <param name="item">工作宿主 [ref]</param>
        /// <returns>返回工作的状态</returns>
        bool DoWorkAsyn(T item);
        /// <summary>
        ///     工作者工作状态汇报事件
        /// </summary>
        event DelegateWorkerProcessing WorkerProcessing;
        /// <summary>
        ///     工作者开始工作事件
        /// </summary>
        event DelegateBeginWork BeginWork;
        /// <summary>
        ///     工作者停止工作事件
        /// </summary>
        event DelegateEndWork EndWork;
    }
}