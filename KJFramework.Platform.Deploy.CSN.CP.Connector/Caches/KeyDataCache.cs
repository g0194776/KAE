using System;
using KJFramework.Platform.Deploy.Metadata.Objects;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector.Caches
{
    public class KeyDataCache : IDataCache<KeyValueItem[]>
    {
        #region Implementation of IDataCache<KeyValueItem[]>

        private string _key;

        private KeyValueItem[] _item;

        private DateTime _lastVisitTime;

        private DateTime _lastUpdateTime;

        /// <summary>
        ///     获取或设置缓存的唯一键值
        /// </summary>
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        /// <summary>
        ///     获取或设置要缓存的项
        /// </summary>
        public KeyValueItem[] Item
        {
            get { return _item; }
            set { _item = value; }
        }

        /// <summary>
        ///     获取或设置最后访问时间
        /// </summary>
        public DateTime LastVisitTime
        {
            get { return _lastVisitTime; }
            set { _lastVisitTime = value; }
        }

        /// <summary>
        ///     获取或设置最后更新时间
        /// </summary>
        public DateTime LastUpdateTime
        {
            get { return _lastUpdateTime; }
            set { _lastUpdateTime = value; }
        }

        #endregion
    }
}