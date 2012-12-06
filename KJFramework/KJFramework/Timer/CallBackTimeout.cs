using System;
using KJFramework.Basic;
using KJFramework.Basic.Enum;
using KJFramework.EventArgs;

namespace KJFramework.Timer
{
    /// <summary>
    ///     提供了所需超时事件的 KFramework.Timer 包装类
    /// </summary>
    public class CallBackTimeout : IMetadata<String>, ITimeout
    {
        #region 成员

        /// <summary>
        ///     超时委托
        /// </summary>
        protected delegate void PublicDelegate();
        /// <summary>
        ///     运行标志位
        /// </summary>
        private int _runFlag;
        /// <summary>
        ///     内部包含的TO_Timer对象
        /// </summary>
        protected Timer _timer = new Timer();
        private TimerStartType _timerstarttype = TimerStartType.RunTimerNow;
        /// <summary>
        ///     超时器运行类型
        ///     默认类型为： 立即启动
        /// </summary>
        public TimerStartType Timerstarttype
        {
            get { return _timerstarttype; }
            set { _timerstarttype = value; }
        }

        #endregion

        #region 构造函数

        /// <summary>
        ///     提供了公共操作的超时计算与处理
        /// </summary>
        /// <param name="key" type="string">
        ///     <para>
        ///         超时器的唯一标示
        ///     </para>
        /// </param>
        /// <param name="interval" type="int">
        ///     <para>
        ///         超时器触发间隔 [单位: 毫秒]
        ///     </para>
        /// </param>
        /// <param name="tryCount" type="int">
        ///     <para>
        ///         超时器总共重试次数,如果为-1则代表永久尝试
        ///     </para>
        /// </param>
        public CallBackTimeout(String key, int interval, int tryCount)
        {
            _timer.TryCount = tryCount;
            _timer.Interval = interval;
            Key = key;
            _timer.Elapsed += TimerElapsed;
            _timer.Timeout += TimerTimeout;
        }

        #endregion

        #region 事件

        void TimerTimeout(object sender, TimeoutEventArgs e)
        {
            _timer.Stop();
            _timer.Close();
            OnOnTryFull(new OnTryFullEventArgs(this));
        }

        void TimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_timerstarttype == TimerStartType.RunTimerNow)
            {
                Run();
            }
            else
            {
                //确保第一次不执行，等到第二次触发间隔到后，还是计算执行
                if (_runFlag > 0)
                {
                    Run();
                }
                else
                {
                    _runFlag++;
                }
            }
        }

        #endregion

        #region 方法

        /// <summary>
        ///     内部超时器运行保证。
        /// </summary>
        private void Run()
        {
            if (_timer.TryCount > 0)
            {
                if (_timer.TryIndex < _timer.TryCount)
                {
                    _timer.TryIndex++;
                }
                else
                {
                    _timer.Stop();
                    _timer.Close();
                    OnOnTryFull(new OnTryFullEventArgs(this));
                }
            }
            //永久尝试
            else
            {
                OnOnTick(new System.EventArgs());
            }
        }

        /// <summary>
        ///     得到当前超时计时器事件类的Timer对象
        /// </summary>
        /// <returns>
        ///     返回Timer对象
        /// </returns>
        public Timer CurrentTimer()
        {
            return _timer;
        }

        #endregion

        #region IMetaData 成员

        private String _key;
        /// <summary>
        ///     获取或设置唯一标示
        /// </summary>
        public string Key
        {
            get
            {
                return _key;
            }
            set
            {
                _key = value;
            }
        }

        #endregion

        #region 'OnTick' event definition code

        // Private delegate linked list (explicitly defined)
        private EventHandler<System.EventArgs> _onTickEventHandlerDelegate;

        /// <summary>
        ///     TODO: Describe the purpose of this event here
        /// </summary>
        public event EventHandler<System.EventArgs> OnTick
        {
            // Explicit event definition with accessor methods
            add
            {
                _onTickEventHandlerDelegate = (EventHandler<System.EventArgs>)System.Delegate.Combine(_onTickEventHandlerDelegate, value);
            }
            remove
            {
                _onTickEventHandlerDelegate = (EventHandler<System.EventArgs>)System.Delegate.Remove(_onTickEventHandlerDelegate, value);
            }
        }

        /// <summary>
        ///     This is the method that is responsible for notifying
        ///     receivers that the event occurred
        /// </summary>
        protected virtual void OnOnTick(System.EventArgs e)
        {
            if (_onTickEventHandlerDelegate != null)
            {
                _onTickEventHandlerDelegate(this, e);
            }
        }

        #endregion //('OnTick' event definition code)

        #region 'OnTryFull' event definition code

        /// <summary>
        ///     EventArgs derived type which holds the custom event fields
        /// </summary>
        public class OnTryFullEventArgs : System.EventArgs
        {
            /// <summary>
            ///     Use this constructor to initialize the event arguments
            ///     object with the custom event fields
            /// </summary>
            public OnTryFullEventArgs(CallBackTimeout timTimerPack)
            {
                TimerPack = timTimerPack;
            }

            /// <summary>
            ///     TODO: Describe the purpose of this event argument here
            /// </summary>
            public readonly CallBackTimeout TimerPack;

        }

        // Private delegate linked list (explicitly defined)
        private EventHandler<OnTryFullEventArgs> _onTryFullEventHandlerDelegate;

        /// <summary>
        ///     TODO: Describe the purpose of this event here
        /// </summary>
        public event EventHandler<OnTryFullEventArgs> OnTryFull
        {
            // Explicit event definition with accessor methods
            add
            {
                _onTryFullEventHandlerDelegate = (EventHandler<OnTryFullEventArgs>)System.Delegate.Combine(_onTryFullEventHandlerDelegate, value);
            }
            remove
            {
                _onTryFullEventHandlerDelegate = (EventHandler<OnTryFullEventArgs>)System.Delegate.Remove(_onTryFullEventHandlerDelegate, value);
            }
        }

        /// <summary>
        ///     This is the method that is responsible for notifying
        ///     receivers that the event occurred
        /// </summary>
        protected virtual void OnOnTryFull(OnTryFullEventArgs e)
        {
            if (_onTryFullEventHandlerDelegate != null)
            {
                _onTryFullEventHandlerDelegate(this, e);
            }
        }

        #endregion //('OnTryFull' event definition code)

        #region ITimeout 成员

        /// <summary>
        ///     开始
        /// </summary>
        public void Start()
        {
            _timer.Start();
        }

        /// <summary>
        ///     停止
        /// </summary>
        public void Stop()
        {
            _timer.Stop();
        }

        /// <summary>
        ///    获取当前超时器的可用状态。
        /// </summary>
        public bool Enable
        {
            get
            {
                return _timer.Enabled;
            }
            set
            {
                _timer.Enabled = value;
            }
        }

        #endregion
    }
}
