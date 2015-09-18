using System;
using System.Collections.Generic;
using System.Linq;

namespace KJFramework.TimingJob
{
    /// <summary>
    ///     指定多时间段(Executing At Specified Time)执行的定时器策略
    ///     <para>比如: 12:00:00;13:00:00</para>
    /// </summary>
    public class EASTTimingJobTimer : ITimingJobTimer
    {
        #region Constructor.

        /// <summary>
        ///     指定多时间段(Executing At Specified Time)执行的定时器策略
        ///     <para>比如: 12:00:00;13:00:00</para>
        /// </summary>
        /// <param name="policy">执行策略信息</param>
        public EASTTimingJobTimer(string policy)
        {
            if (string.IsNullOrEmpty(policy)) throw new ArgumentNullException(nameof(policy));
            TimeSpan[] timeSpans = policy.Split(new[] {";"}, StringSplitOptions.RemoveEmptyEntries).Select(v => TimeSpan.Parse(v)).ToArray();
            _executingIntervals = new Dictionary<int, List<TimeSpan>>();
            foreach (TimeSpan timeSpan in timeSpans)
            {
                List <TimeSpan > tmp;
                if (!_executingIntervals.TryGetValue(timeSpan.Hours, out tmp))
                    _executingIntervals[timeSpan.Hours] = tmp = new List<TimeSpan>();
                tmp.Add(timeSpan);
            }
        }

        #endregion

        #region Implementation of ITimingJobTimer

        private readonly Dictionary<int, List<TimeSpan>>  _executingIntervals;

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
                List<TimeSpan> tmp;
                if (!_executingIntervals.TryGetValue(now.Hour, out tmp)) return false;
                foreach (TimeSpan timeSpan in tmp)
                {
                    if (now.Hour == timeSpan.Hours
                        && now.Minute == timeSpan.Minutes
                        && now.Second - timeSpan.Seconds <= 5)
                        return true;
                }
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