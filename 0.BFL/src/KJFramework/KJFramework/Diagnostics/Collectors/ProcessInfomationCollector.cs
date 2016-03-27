using System;
using System.Diagnostics;
using KJFramework.EventArgs;

namespace KJFramework.Diagnostics.Collectors
{
    /// <summary>
    ///     进程信息收集器， 提供了相关的基本操作
    /// </summary>
    public class ProcessInfomationCollector : InfomationCollector
    {
        #region 构造函数

        /// <summary>
        ///     进程信息收集器， 提供了相关的基本操作
        /// </summary>
        /// <param name="reviewere">审查器</param>
        public ProcessInfomationCollector(IInfomationReviewer reviewere)
            : base(Process.GetCurrentProcess().GetType(), reviewere)
        {
            IntervalTimeChanged += ThreadInfomationCollectorIntervalTimeChanged;
        }

        #endregion

        #region 成员

        protected String _infomation;

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
            _infomation =
                String.Format(
                    "[Process] Process Id : {0}, Process Name : {1}\r\n----------------------------------------------------------------------\r\nTotalProcessorTime : {2}(s)\r\nUserProcessorTime : {3}(s)\r\nPrivilegedProcessorTime : {4}(s)\r\nPagedSystemMemorySize64 : {5}\r\nPagedMemorySize64 : {6}\r\nPrivateMemorySize64 : {7}\r\nPeakVirtualMemorySize64 : {8}\r\n",
                    Process.GetCurrentProcess().Id, Process.GetCurrentProcess().ProcessName,
                    Process.GetCurrentProcess().TotalProcessorTime.TotalSeconds,
                    Process.GetCurrentProcess().UserProcessorTime.TotalSeconds,
                    Process.GetCurrentProcess().PrivilegedProcessorTime.TotalSeconds,
                    Process.GetCurrentProcess().PagedSystemMemorySize64,
                    Process.GetCurrentProcess().PagedMemorySize64,
                    Process.GetCurrentProcess().PrivateMemorySize64,
                    Process.GetCurrentProcess().PeakVirtualMemorySize64);
            NewInfomationHandler(new NewInfomationEventArgs(_infomation));
        }

        #endregion
    }
}