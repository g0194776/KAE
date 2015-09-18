using System;

namespace KJFramework.TimingJob.EventArgs
{
    /// <summary>
    ///    任务接收事件参数
    /// </summary>
    public class TaskRecvEventArgs<T> : System.EventArgs
    {
        #region Constructor.

        /// <summary>
        ///    任务接收事件参数
        /// </summary>
        /// <param name="result">接收的任务结果对象</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public TaskRecvEventArgs(T result)
        {
            if (result == null) throw new ArgumentNullException("result");
            TaskResult = result;
        }

        #endregion

        #region Members.

        /// <summary>
        ///    获取接收到的任务结果对象
        /// </summary>
        public T TaskResult { get; internal set; }

        #endregion
    }
}