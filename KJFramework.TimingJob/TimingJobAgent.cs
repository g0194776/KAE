using System;
using KJFramework.Results;

namespace KJFramework.TimingJob
{
    /// <summary>
    ///     定时任务代理器
    /// </summary>
    public static class TimingJobAgent
    {
        #region Methods.

        /// <summary>
        ///     获取指定TimingJob的最后执行结果
        /// </summary>
        /// <param name="timingJobName">TimingJob的全局名称</param>
        /// <param name="ignoreCache">
        ///     是否忽略缓存中的结果
        ///     <para>* 如果忽略，则需要重新执行指定TimingJob并等待获取结果</para>
        /// </param>
        /// <returns>返回执行的结果</returns>
        public static IExecuteResult<string> GetLastResult(string timingJobName, bool ignoreCache = false)
        {
            return GetLastResult(timingJobName, ignoreCache, TimeSpan.MinValue);
        }

        /// <summary>
        ///     获取指定TimingJob的最后执行结果
        /// </summary>
        /// <param name="timingJobName">TimingJob的全局名称</param>
        /// <param name="ignoreCache">
        ///     是否忽略缓存中的结果
        ///     <para>* 如果忽略，则需要重新执行指定TimingJob并等待获取结果</para>
        /// </param>
        /// <param name="ignoreCacheInterval">
        ///     此参数仅当ignoreCache = true时生效。
        ///     <para>* 此参数的用途是忽略多长时间之外的缓存结果</para>
        /// </param>
        /// <returns>返回执行的结果</returns>
        public static IExecuteResult<string> GetLastResult(string timingJobName, bool ignoreCache, TimeSpan ignoreCacheInterval)
        {
            return null;
        }

        #endregion
    }
}