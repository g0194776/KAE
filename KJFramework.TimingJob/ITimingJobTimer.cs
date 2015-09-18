using System;

namespace KJFramework.TimingJob
{
    /// <summary>
    ///     定时任务计数器接口
    /// </summary>
    public interface ITimingJobTimer
    {
        #region Members.

        /// <summary>
        ///     获取最后一次执行时间
        /// </summary>
        DateTime LastExecutingTime { get; }
        /// <summary>
        ///     获取一个状态值，该值表示了当前的定时任务是否应该执行的标识
        /// </summary>
        bool CanExecuting { get; }

        #endregion

        #region Methods.

        /// <summary>
        ///    更新内部最后一次执行的时间
        /// </summary>
        void UpdateTime();

        #endregion
    }
}