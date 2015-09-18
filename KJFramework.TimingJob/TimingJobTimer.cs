using System;

namespace KJFramework.TimingJob
{
    /// <summary>
    ///     定时任务执行策略生成器
    /// </summary>
    public static class TimingJobTimer
    {
        #region Methods.

        /// <summary>
        ///     通过指定的策略来创建一个定器时任务执行策略
        /// </summary>
        /// <param name="policy">定时执行策略</param>
        /// <returns>返回所配匹的定器时任务执行策略</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        /// <exception cref="NotSupportedException">不支持的策略</exception>
        /// <exception cref="ArgumentException">无效参数</exception>
        public static ITimingJobTimer New(string policy)
        {
            if (string.IsNullOrEmpty(policy)) throw new ArgumentNullException(nameof(policy));
            string[] arguments = policy.Split(new[] {"#"}, StringSplitOptions.RemoveEmptyEntries);
            if(arguments.Length != 2) throw new ArgumentException("#Illegal input argument.");
            switch (arguments[0].ToUpper())
            {
                case "FEI":
                    return new FEITimingJobTimer(arguments[1]);
                case "EAST":
                    return new EASTTimingJobTimer(arguments[1]);
                case "FET":
                    return new FETTimingJobTimer(arguments[1]);
                default:
                    throw new NotSupportedException(policy);
            }
        }

        #endregion
    }
}