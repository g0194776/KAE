using KJFramework.Engin.Scheduler;
using KJFramework.Engin.Worker;

namespace KJFramework.Engin
{
    /// <summary>
    ///     工作者工作引擎，提供了相关的基本操作。
    /// </summary>
    public interface IWorkerEngin<TWorker, THost> : IEngin
        where TWorker : IWorker<THost>
    {
        /// <summary>
        ///     最多支持的调度者数量
        ///         * 默认为 5
        /// </summary>
        int SchedulerCount { get; set; }
        /// <summary>
        ///     添加调度者
        /// </summary>
        /// <param name="scheduler">新的调度者</param>
        /// <returns>返回加入的状态</returns>
        bool AddScheduler(IWorkerScheduler<TWorker, THost> scheduler);
        /// <summary>
        ///     添加工作者
        /// </summary>
        /// <param name="worker">新的工作者</param>
        /// <returns>返回添加的状态</returns>
        bool AddWorker(TWorker worker);
    }
}