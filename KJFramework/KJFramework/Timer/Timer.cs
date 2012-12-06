using KJFramework.EventArgs;

namespace KJFramework.Timer
{
    /// <summary>
    ///     提供了支持2种超时策略的超时器。
    /// </summary>
    public class Timer : System.Timers.Timer, ITimer
    {
        #region ITimer 成员

        private int _trycount;

        /// <summary>
        ///     尝试总数
        /// </summary>
        public int TryCount
        {
            get
            {
                return _trycount;
            }
            set
            {
                _trycount = value;
            }
        }

        private int _tryindex;

        /// <summary>
        ///     当前尝试次数
        /// </summary>
        public int TryIndex
        {
            get { return _tryindex; }
            set { _tryindex = value; }
        }

        /// <summary>
        ///      超时器超时事件
        /// </summary>
        /// <remarks>
        ///     当已经到达所指定的尝试次数，仍然未满足指定条件，则会触发该事件。
        /// </remarks>
        public event DELEGATE_TIMEOUT Timeout;
        private void TimeoutHandler(TimeoutEventArgs e)
        {
            if (Timeout != null)
            {
                Timeout(this, e);
            }
        }
        #endregion
    }
}
