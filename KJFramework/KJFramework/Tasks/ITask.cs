using System;
using KJFramework.Enums;

namespace KJFramework.Tasks
{
    /// <summary>
    ///     任务元接口，提供了相关的基本操作。
    /// </summary>
    public interface ITask : IDisposable
    {
        /// <summary>
        ///     获取或设置任务描述
        /// </summary>
        String Description { get; set; }
        /// <summary>
        ///     获取或设置任务唯一标示
        /// </summary>
        int Id { get; set; }
        /// <summary>
        ///     获取一个值，该值表示了当前任务是否已经完成
        /// </summary>
        bool IsFinished { get; }
        /// <summary>
        ///     获取一个值，该值表示了当前任务是否已经取消
        /// </summary>
        bool IsCanceled { get; }
        /// <summary>
        ///     获取任务创建时间
        /// </summary>
        DateTime CreateTime { get; }
        /// <summary>
        ///     获取或设置任务过期时间
        ///             * 如果设置为null, 则表示永远不过期
        /// </summary>
        DateTime? ExpiredTime { get; set; }
        /// <summary>
        ///     获取或设置任务优先级
        /// </summary>
        TaskPriority Priority { get; set; }
        /// <summary>
        ///     取消任务
        /// </summary>
        void Cancel();
        /// <summary>
        ///     执行任务
        /// </summary>
        void Execute();
        /// <summary>
        ///     异步执行任务
        /// </summary>
        void ExecuteAsyn();
        event EventHandler ExecuteSuccessful , ExecuteFail;
    }
}