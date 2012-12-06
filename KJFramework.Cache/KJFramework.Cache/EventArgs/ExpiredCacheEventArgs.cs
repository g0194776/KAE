namespace KJFramework.Cache.EventArgs
{
    /// <summary>
    ///     �����ѹ����¼�
    /// </summary>
    /// <typeparam name="K">�������Key����</typeparam>
    /// <typeparam name="V">�����������</typeparam>
    public class ExpiredCacheEventArgs<K, V> : System.EventArgs
    {
        #region Members

        private readonly K _key;
        private readonly V _obj;
        /// <summary>
        ///     �������Key
        /// </summary>
        public K Key
        {
            get { return _key; }
        }
        /// <summary>
        ///     �������
        /// </summary>
        public V Obj
        {
            get { return _obj; }
        }

        #endregion

        #region Constructor

        /// <summary>
        ///     �����ѹ����¼�
        /// </summary>
        /// <param name="key">�������Key</param>
        /// <param name="obj">�������</param>
        public ExpiredCacheEventArgs(K key, V obj)
        {
            _key = key;
            _obj = obj;
        }

        #endregion
    }
}