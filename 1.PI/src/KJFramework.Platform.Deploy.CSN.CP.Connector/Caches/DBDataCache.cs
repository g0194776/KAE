using System;
using KJFramework.Platform.Deploy.Metadata.Objects;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector.Caches
{
    /// <summary>
    ///     数据库数据缓存对象
    /// </summary>
    public class DBDataCache : IDataCache<DataTable>
    {
        #region Constructor

        /// <summary>
        ///     数据库数据缓存对象
        /// </summary>
        public DBDataCache()
        {
            _lastUpdateTime = _lastVisitTime =  DateTime.Now;
        }

        #endregion

        #region Implementation of IDataCache<DataTable>

        protected string _key;
        protected DataTable _item;
        protected DateTime _lastVisitTime;
        protected DateTime _lastUpdateTime;

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
        public DataTable Item
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