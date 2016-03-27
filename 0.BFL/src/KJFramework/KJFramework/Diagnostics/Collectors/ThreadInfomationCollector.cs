using System;
using System.Threading;
using System.Diagnostics;
using KJFramework.EventArgs;

namespace KJFramework.Diagnostics.Collectors
{
    /// <summary>
    ///     �߳���Ϣ��Ϣ�ռ������ṩ����صĻ���������
    /// </summary>
    public class ThreadInfomationCollector : InfomationCollector
    {
        #region ���캯��

        public ThreadInfomationCollector(IInfomationReviewer reviewere)
            : base(Thread.CurrentThread.GetType(), reviewere)
        {
            IntervalTimeChanged += ThreadInfomationCollectorIntervalTimeChanged;
        }

        #endregion

        #region ���෽��

        /// <summary>
        ///     ��ʼִ��
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
        ///     ִֹͣ��
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

        #region ��Ա

        protected String _infomation;

        #endregion

        #region �¼�

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