using System;

namespace KJFramework.TimingJob
{
    /// <summary>
    ///     固定时间(Fixed Executing Time)执行的定时器策略
    ///     <para>比如: 13:00:00</para>
    /// </summary>
    public class FETTimingJobTimer : ITimingJobTimer
    {
        #region Constructor.

        /// <summary>
        ///     固定时间(Fixed Executing Time)执行的定时器策略
        ///     <para>比如: 13:00:00</para>
        /// </summary>
        /// <param name="policy">执行策略信息</param>
        public FETTimingJobTimer(string policy)
        {
            if (string.IsNullOrEmpty(policy)) throw new ArgumentNullException(nameof(policy));
            _executingInterval = TimeSpan.Parse(policy);
        }

        #endregion

        #region Implementation of ITimingJobTimer

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
            get
            {
                DateTime now = DateTime.Now;
                if (now.Hour == _executingInterval.Hours 
                    && now.Minute == _executingInterval.Minutes 
                    && now.Second - _executingInterval.Seconds <= 5) return true;
                return false;
            }
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