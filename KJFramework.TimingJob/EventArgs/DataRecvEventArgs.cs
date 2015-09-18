using System;

namespace KJFramework.TimingJob.EventArgs
{
    /// <summary>
    ///    数据接收事件参数
    /// </summary>
    public class DataRecvEventArgs : System.EventArgs
    {
        #region Constructor.

        /// <summary>
        ///    数据接收事件参数
        /// </summary>
        /// <param name="obj">接收的数据</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public DataRecvEventArgs(object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            Data = obj;
        }

        #endregion

        #region Members.

        /// <summary>
        ///    获取接收到的数据
        /// </summary>
        public object Data { get; internal set; }

        #endregion
    }
}