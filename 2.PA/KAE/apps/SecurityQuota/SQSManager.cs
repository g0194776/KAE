using System;
using System.Collections.Generic;
using System.Threading;

namespace SecurityQuota
{
    /// <summary>
    ///		SQS管理器
    /// </summary>
    public class SQSManager
    {
        #region Members.

        private Thread _thread;
        private readonly TimeSpan _sleepInterval;
        private readonly object _quotaCacheLock = new object();
        private readonly IDictionary<string, QuotaCacheItem> _quotaCache = new Dictionary<string, QuotaCacheItem>();

        #endregion

        #region Constructor.

        /// <summary>
        ///		本地缓存管理
        /// </summary>
        /// <param name="sleepInterval">清理失效缓存时间间隔，单位为分钟，默认30分钟</param>
        public SQSManager(int sleepInterval = 30)
        {
            _sleepInterval = new TimeSpan(0, sleepInterval, 0);
            _thread = new Thread(delegate()
            {
                while (true)
                {
                    Thread.Sleep(_sleepInterval);
                    lock (_quotaCacheLock)
                    {
                        IList<string> expiredKeys = new List<string>();
                        foreach (KeyValuePair<string, QuotaCacheItem> pair in _quotaCache)
                            if (pair.Value.IsExpire) expiredKeys.Add(pair.Key);
                        if (expiredKeys.Count > 0)
                        {
                            foreach (string key in expiredKeys)
                                _quotaCache.Remove(key);
                        }
                    }
                }
            });
            _thread.IsBackground = true;
            _thread.Start();
        }

        #endregion

        #region Methods.

        /// <summary>
        ///     判断指定服务业务达到配额规则
        /// </summary>
        /// <param name="serviceNme">指定服务业务名</param>
        /// <param name="rule">配额规则,格式为：次数/秒数，例如：2/1代表2秒1次</param>
        /// <returns>超过限制规则返回true</returns>
        public bool Check(string serviceNme, string rule)
        {
            if (string.IsNullOrEmpty(rule.Trim())) return false;
            QuotaCacheItem cacheItem;
            if (_quotaCache.TryGetValue(serviceNme, out cacheItem)) return cacheItem.IsUltralimit(rule);
            cacheItem = new QuotaCacheItem();
            lock (_quotaCacheLock)
            {
                _quotaCache[serviceNme] = cacheItem;
            }
            return cacheItem.IsUltralimit(rule);
        }

        #endregion
    }
}