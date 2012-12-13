using KJFramework.Tracing;
using System;
using System.Timers;

namespace KJFramework.Timer
{
    /// <summary>
    ///     ��������ʱ�����ṩ����صĻ�������
    /// </summary>
    public class LightTimer
    {
        #region ���캯��

        /// <summary>
        ///     ��������ʱ�����ṩ����صĻ�������
        /// </summary>
        /// <param name="interval">�������</param>
        /// <param name="timeoutCount">��ʱ����</param>
        public LightTimer(int interval, int timeoutCount)
        {
            _interval = interval;
            _timeoutCount = timeoutCount;
        }

        #endregion

        #region ��Ա

        private int _interval;
        private int _timeoutCount = 1;
        private int _currentTimeoutCount;
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(LightTimer));

        /// <summary>
        ///     ��ȡ�����ü��ʱ��
        ///         * ʱ�䣺����
        /// </summary>
        public int Interval
        {
            get { return _interval; }
            set
            {
                _interval = value;
                if (_timer != null)
                {
                    _timer.Interval = _interval;
                }
            }
        }

        /// <summary>
        ///     ��ȡ�����ó�ʱ����
        ///         * ʱ�䣺����
        /// </summary>
        public int TimeoutCount
        {
            get { return _timeoutCount; }
            set { _timeoutCount = value; }
        }

        /// <summary>
        ///     ��ȡ������ʱ��������
        /// </summary>
        public Action IntervalAction
        {
            get { return _intervalAction; }
            set { _intervalAction = value; }
        }

        /// <summary>
        ///     ��ȡ�����õ���ָ����ʱ��������
        /// </summary>
        public Action TimeoutAction
        {
            get { return _timeoutAction; }
            set { _timeoutAction = value; }
        }

        private System.Timers.Timer _timer;
        private Action _intervalAction;
        private Action _timeoutAction;

        #endregion

        #region ����

        /// <summary>
        ///     ����һ���µ���������ʱ��
        /// </summary>
        /// <param name="interval">�������</param>
        /// <param name="timeoutCount">��ʱ����</param>
        public static LightTimer NewTimer(int interval, int timeoutCount)
        {
            return new LightTimer(interval, timeoutCount);
        }

        /// <summary>
        ///     ��ʼ
        /// </summary>
        /// <param name="intervalAction">��������</param>
        /// <param name="timeoutAction">��ʱ����</param>
        public LightTimer Start(Action intervalAction, Action timeoutAction)
        {
            _intervalAction = intervalAction;
            _timeoutAction = timeoutAction;
            if (_timer == null)
            {
                _timer = new System.Timers.Timer(_interval);
                _timer.Elapsed += TimerElapsed;
                _timer.Start();
            }
            return this;
        }

        /// <summary>
        ///     ֹͣ
        /// </summary>
        public void Stop()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Elapsed -= TimerElapsed;
                _timer = null;
            }
            _intervalAction = null;
            _timeoutAction = null;
        }

        #endregion

        #region �¼�

        void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                //���ó���
                if (_timeoutCount == -1)
                {
                    if (_intervalAction != null)
                    {
                        _intervalAction();
                    }
                    return;
                }
                if (_timeoutCount == 1 || ++_currentTimeoutCount >= _timeoutCount)
                {
                    if (_timeoutAction != null)
                    {
                        _timeoutAction();
                    }
                    Stop();
                }
                else
                {
                    if (_intervalAction != null)
                    {
                        _intervalAction();
                    }
                    _currentTimeoutCount++;
                }
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
            }
        }

        #endregion
    }
}