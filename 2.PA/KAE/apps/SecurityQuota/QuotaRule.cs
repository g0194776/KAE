using System;

namespace SecurityQuota
{
    /// <summary>
    ///		配额规则对象
    /// </summary>
    internal class QuotaRule
    {
        #region members.

        /// <summary>
        ///		获取或设置规定时间内配额允许的次数
        /// </summary>
        public int Frequence { get; set; }
        /// <summary>
        ///		获取或设置配额监控的时间段
        /// </summary>
        public TimeSpan Interval { get; set; }

        #endregion
    }
}