using System;

namespace KJFramework.TimingJob
{
    /// <summary>
    ///     固定时间间隔(Fixed Executing Interval)执行的定时器策略
    ///     <para>比如: 3600s</para>
    /// </summary>
    public class FEITimingJobTimer : ITimingJobTimer
    {
        #region Constructor.

        /// <summary>
        ///     固定时间间隔(Fixed Executing Interval)执行的定时器策略
        ///     <para>比如: 3600s</para>
        /// </summary>
        /// <param name="policy">执行策略信息</param>
        public FEITimingJobTimer(string policy)
        {
            if (string.IsNullOrEmpty(policy)) throw new ArgumentNullException(nameof(policy));
            if (policy.EndsWith("s", StringComparison.CurrentCultureIgnoreCase))
                _executingInterval = new TimeSpan(0, 0, int.Parse(policy.Remove(policy.Length - 1)));
            else if (policy.EndsWith("m", StringComparison.CurrentCultureIgnoreCase))
                _executingInterval = new TimeSpan(0, 0, int.Parse(policy.Remove(policy.Length - 1))*60);
            else if (policy.EndsWith("h", StringComparison.CurrentCultureIgnoreCase))
                _executingInterval = new TimeSpan(0, 0, int.Parse(policy.Remove(policy.Length - 1)) * 3600);
            throw new NotSupportedException(policy);
        }

        #endregion

        #region Members.

        private readonly TimeSpan _executingInterval;
        /// <summary>
        ///     获取最后一次执行时间
        /// </summary>
        public DateTime LastExecutingTime { get; private set; }

        /// <summary>
        ///     获取一个状态值，该值表示了当前的定时任务是否应该执行的标识
        /// </summary>
        public bool CanExecuting
        {
            get { return (DateTime.Now - LastExecutingTime) > _executingInterval; }
        }

        #endregion

        #region Methods.

        /// <summary>
        ///    更新内部最后一次执行的时间
        /// </summary>
        public void UpdateTime()
        {
            LastExecutingTime = DateTime.Now;
        }

        #endregion
    }
}