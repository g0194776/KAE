using KJFramework.Results;

namespace KJFramework.TimingJob
{
    /// <summary>
    ///     KJFramework中被支持的定时任务基础接口
    /// </summary>
    public interface ITimingJob
    {
        #region Methods.

        /// <summary>
        ///     执行当前的定时任务
        /// </summary>
        /// <param name="manager">全局通知管理器</param>
        /// <param name="args">执行时需要传递的参数</param>
        /// <returns>返回执行后的状态</returns>
        IExecuteResult<string> Do(INotificationManager manager, string[] args);

        #endregion
    }
}