namespace KJFramework.Data.Synchronization.EventArgs
{
    /// <summary>
    ///     接受数据事件
    /// </summary>
    /// <typeparam name="K">key类型</typeparam>
    /// <typeparam name="V">value类型</typeparam>
    public class DataRecvEventArgs<K, V> : System.EventArgs
    {
        #region Members

        public K Key { get; private set; }
        public V Value { get; private set; }
        public string Catalog { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        ///     接受数据事件
        /// </summary>
        /// <param name="catalog">分组名称</param>
        /// <param name="key">关键字</param>
        /// <param name="value">相关值</param>
        public DataRecvEventArgs(string catalog, K key, V value)
        {
            Key = key;
            Value = value;
            Catalog = catalog;
        }

        #endregion
    }
}