using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SecurityQuota
{
    /// <summary>
    ///     配额业务缓存
    /// </summary>
    internal class QuotaCacheItem
    {
        #region Members.

        private string _ruleStr;
        private readonly QuotaRule _rule;
        private readonly Object _cacheItemLock;
        private readonly List<DateTime> _quotaCache;
        private static readonly Regex _regx = new Regex(@"^\d+/\d+[smh]*$");

        /// <summary>
        ///		获取缓存是否过期
        /// </summary>
        public bool IsExpire
        {
            get
            {
                if (_quotaCache.Count == 0) return false;
                return (_quotaCache[_quotaCache.Count - 1] + _rule.Interval) < DateTime.Now;
            }
        }

        #endregion

        #region Constructor.

        /// <summary>
        ///     配额业务缓存
        /// </summary>
        public QuotaCacheItem()
        {
            _cacheItemLock = new object();
            _quotaCache = new List<DateTime>();
            _rule = new QuotaRule();
        }

        #endregion

        #region Methods.

        /// <summary>
        ///		判断指定服务业务达到配额规则
        /// </summary>
        /// <param name="rule">配额规则,格式为：次数/秒数，例如：1/2代表2秒1次</param>
        /// <returns>超过限制规则返回true</returns>
        public bool IsUltralimit(string rule)
        {
            if (!_regx.IsMatch(rule)) return false;
            lock (_cacheItemLock)
            {
                if (!rule.Trim().Equals(_ruleStr))
                {
                    string[] args = rule.Trim().Split('/');
                    int frequence = int.Parse(args[0]);
                    TimeSpan interval = parseTimeSpan(args[1].Trim());
                    if (_rule.Frequence != frequence) _rule.Frequence = frequence;
                    if (_rule.Interval != interval) _rule.Interval = interval;
                    _ruleStr = rule.Trim();
                }
                if (IsExpire)
                {
                    _quotaCache.Clear();
                    _quotaCache.Add(DateTime.Now);
                    return false;
                }
                int deleteIndex = _quotaCache.FindLastIndex(time => time + _rule.Interval < DateTime.Now);
                _quotaCache.RemoveRange(0, deleteIndex + 1);
                if (_quotaCache.Count >= _rule.Frequence) return true;
                _quotaCache.Add(DateTime.Now);
                return false;
            }
        }

        /// <summary>
        ///		解析时间间隔字符串
        /// </summary>
        /// <param name="timeStr">支持s,m,h三种时间单位，不给定单位则默认单位为秒</param>
        /// <returns>返回TimeSpan类型的时间间隔对象</returns>
        private TimeSpan parseTimeSpan(string timeStr)
        {
            if (timeStr.EndsWith("s")) return new TimeSpan(0, 0, int.Parse(timeStr.Substring(0, timeStr.Length - 1)));
            if (timeStr.EndsWith("m")) return new TimeSpan(0, int.Parse(timeStr.Substring(0, timeStr.Length - 1)), 0);
            if (timeStr.EndsWith("h")) return new TimeSpan(int.Parse(timeStr.Substring(0, timeStr.Length - 1)), 0, 0);
            return new TimeSpan(0, 0, int.Parse(timeStr));
        }

        #endregion
    }
}