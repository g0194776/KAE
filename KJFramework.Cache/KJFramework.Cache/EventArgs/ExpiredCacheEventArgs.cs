namespace KJFramework.Cache.EventArgs
{
    /// <summary>
    ///     缓存已过期事件
    /// </summary>
    /// <typeparam name="K">缓存对象Key类型</typeparam>
    /// <typeparam name="V">缓存对象类型</typeparam>
    public class ExpiredCacheEventArgs<K, V> : System.EventArgs
    {
        #region Members

        private readonly K _key;
        private readonly V _obj;
        /// <summary>
        ///     缓存对象Key
        /// </summary>
        public K Key
        {
            get { return _key; }
        }
        /// <summary>
        ///     缓存对象
        /// </summary>
        public V Obj
        {
            get { return _obj; }
        }

        #endregion

        #region Constructor

        /// <summary>
        ///     缓存已过期事件
        /// </summary>
        /// <param name="key">缓存对象Key</param>
        /// <param name="obj">缓存对象</param>
        public ExpiredCacheEventArgs(K key, V obj)
        {
            _key = key;
            _obj = obj;
        }

        #endregion
    }
}