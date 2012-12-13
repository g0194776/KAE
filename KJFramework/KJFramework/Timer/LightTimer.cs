using KJFramework.Tracing;
using System;
using System.Timers;

namespace KJFramework.Timer
{
    /// <summary>
    ///     轻量级超时器，提供了相关的基本操作
    /// </summary>
    public class LightTimer
    {
        #region 构造函数

        /// <summary>
        ///     轻量级超时器，提供了相关的基本操作
        /// </summary>
        /// <param name="interval">触发间隔</param>
        /// <param name="timeoutCount">超时次数</param>
        public LightTimer(int interval, int timeoutCount)
        {
            _interval = interval;
            _timeoutCount = timeoutCount;
        }

        #endregion

        #region 成员

        private int _interval;
        private int _timeoutCount = 1;
        private int _currentTimeoutCount;
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(LightTimer));

        /// <summary>
        ///     获取或设置间隔时间
        ///         * 时间：毫秒
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
        ///     获取或设置超时次数
        ///         * 时间：毫秒
        /// </summary>
        public int TimeoutCount
        {
            get { return _timeoutCount; }
            set { _timeoutCount = value; }
        }

        /// <summary>
        ///     获取或设置时间间隔动作
        /// </summary>
        public Action IntervalAction
        {
            get { return _intervalAction; }
            set { _intervalAction = value; }
        }

        /// <summary>
        ///     获取或设置到达指定超时次数动作
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

        #region 方法

        /// <summary>
        ///     创建一个新的轻量级超时器
        /// </summary>
        /// <param name="interval">触发间隔</param>
        /// <param name="timeoutCount">超时次数</param>
        public static LightTimer NewTimer(int interval, int timeoutCount)
        {
            return new LightTimer(interval, timeoutCount);
        }

        /// <summary>
        ///     开始
        /// </summary>
        /// <param name="intervalAction">触发动作</param>
        /// <param name="timeoutAction">超时动作</param>
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
        ///     停止
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

        #region 事件

        void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                //永久尝试
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