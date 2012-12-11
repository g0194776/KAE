using System;
using System.Collections.Generic;
using KJFramework.Logger;
using KJFramework.Platform.Deploy.CSN.Common.Configurations;

namespace KJFramework.Platform.Deploy.CSN.Common.Caches
{
    /// <summary>
    ///     数据缓存管理器，提供了相关的基本操作。
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public class DataCacheManager<T>
    {
        #region Constructor

        /// <summary>
        ///     数据缓存管理器，提供了相关的基本操作。
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        public DataCacheManager()
        {
            _liveTime = TimeSpan.Parse(CSNSettingConfigSection.Current.Settings.CacheLiveTime);
            _timer = new System.Timers.Timer(CSNSettingConfigSection.Current.Settings.CacheTimeoutCheckInterval);
            _timer.Elapsed += Elapsed;
            _timer.Start();
        }

        #endregion

        #region Members

        private Dictionary<string, IDataCache<T>> _caches = new Dictionary<string, IDataCache<T>>();
        private object _lockObj = new object();
        private System.Timers.Timer _timer;
        private TimeSpan _liveTime;

        #endregion

        #region Methods

        /// <summary>
        ///     获取具有唯一键值的缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IDataCache<T> GetCache(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(key);
            }
            lock (_lockObj)
            {
                IDataCache<T> cache;
                if (_caches.TryGetValue(key, out cache))
                {
                    cache.LastVisitTime = DateTime.Now;
                    return cache;
                }
            }
            return null;
        }

        /// <summary>
        ///     添加一个缓存
        /// </summary>
        /// <param name="cache">数据缓存</param>
        public void Add(IDataCache<T> cache)
        {
            if (cache.Key == null)
            {
                throw new ArgumentException("Invaild args. #IDataCache.Key is null.");
            }
            lock (_lockObj)
            {
                if (_caches.ContainsKey(cache.Key))
                {
                    _caches.Remove(cache.Key);
                }
                _caches.Add(cache.Key ,cache);
            }
        }

        #endregion

        #region Events

        //timeout check timer.
        void Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (_lockObj)
            {
                try
                {
                    if (_caches.Count == 0)
                    {
                        return;
                    }
                    DateTime now = DateTime.Now;
                    List<string> timeoutValues = new List<string>();
                    foreach (KeyValuePair<string, IDataCache<T>> keyValuePair in _caches)
                    {
                        if ((now - keyValuePair.Value.LastVisitTime).TotalSeconds >= _liveTime.TotalSeconds)
                        {
                            timeoutValues.Add(keyValuePair.Key);
                        }
                    }
                    foreach (string key in timeoutValues)
                    {
                        _caches.Remove(key);
                    }
                }
                catch (System.Exception ex)
                {
                    Logs.Logger.Log(ex);
                }
            }
        }

        #endregion
    }
}