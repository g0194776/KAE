using System.Collections.Generic;
using KJFramework.Basic.Enum;
using KJFramework.Engin.Worker;

namespace KJFramework.Engin.Scheduler.Rule
{
    /// <summary>
    ///     工作者调度规则，提供了相关的调度规范。
    /// </summary>
    public interface IWorkerSchedulerRule<TWorker, THost>
        where TWorker : IWorker<THost>
    {
        /// <summary>
        ///     获取调度优先级
        /// </summary>
        SchedulerPriority Priority { get; }
        /// <summary>
        ///     调度检测，仅返回调度器自身需要的工作者集合
        /// </summary>
        /// <param name="worker">工作者集合</param>
        /// <returns>返回需要的工作者集合</returns>
        List<TWorker> Check(List<TWorker> worker);
    }
}