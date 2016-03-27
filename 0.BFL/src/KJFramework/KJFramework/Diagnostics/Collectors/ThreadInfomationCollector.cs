using System;
using System.Threading;
using System.Diagnostics;
using KJFramework.EventArgs;

namespace KJFramework.Diagnostics.Collectors
{
    /// <summary>
    ///     线程信息信息收集器，提供了相关的基本操作。
    /// </summary>
    public class ThreadInfomationCollector : InfomationCollector
    {
        #region 构造函数

        public ThreadInfomationCollector(IInfomationReviewer reviewere)
            : base(Thread.CurrentThread.GetType(), reviewere)
        {
            IntervalTimeChanged += ThreadInfomationCollectorIntervalTimeChanged;
        }

        #endregion

        #region 父类方法

        /// <summary>
        ///     开始执行
        /// </summary>
        public override void Start()
        {
            if (_timer == null)
            {
                _timer = new System.Timers.Timer(_collectInterval);
                _timer.Elapsed += TimerElapsed;
                _timer.Start();
            }
            base.Start();
        }

        /// <summary>
        ///     停止执行
        /// </summary>
        public override void Stop()
        {
            if (_timer != null)
            {
                _timer.Elapsed -= TimerElapsed;
                _timer.Stop();
            }
            base.Stop();
        }

        #endregion

        #region 成员

        protected String _infomation;

        #endregion

        #region 事件

        void ThreadInfomationCollectorIntervalTimeChanged(object sender, System.EventArgs e)
        {
            if (_timer != null)
            {
                _timer.Interval = _collectInterval;
            }
        }

        void TimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _infomation = String.Format("[Thread] Thread Id : {0}, Current State : {1}, Priority : {2}, Threads Count : {3}",
                                        Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.ThreadState,
                                        Thread.CurrentThread.Priority, Process.GetCurrentProcess().Threads.Count);
            NewInfomationHandler(new NewInfomationEventArgs(_infomation));
        }

        #endregion
    }
}