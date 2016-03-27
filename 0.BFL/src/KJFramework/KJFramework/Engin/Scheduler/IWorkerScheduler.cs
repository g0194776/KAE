using System;
using System.Collections.Generic;
using KJFramework.Engin.Scheduler.Rule;
using KJFramework.Engin.Worker;

namespace KJFramework.Engin.Scheduler
{
    /// <summary>
    ///     工作者调度器元接口，提供了相关的调度工作。
    /// </summary>
    /// <typeparam name="TWorker">工作者类型</typeparam>
    /// <typeparam name="THost">工作者所需要的宿主类型</typeparam>
    public interface IWorkerScheduler<TWorker, THost> : IDisposable
        where TWorker : IWorker<THost>
    {
        /// <summary>
        ///     获取一个值，该值表示了当前的调度器是否已经完成了全部的调度工作
        /// </summary>
        bool IsFinish { get; }
        /// <summary>
        ///     获取调度规则
        /// </summary>
        IWorkerSchedulerRule<TWorker, THost> Rule { get; }
        /// <summary>
        ///     执行调度工作
        /// </summary>
        /// <param name="worker">要调度的工作者集合</param>
        void Dispatcher(List<TWorker> worker);
    }
}