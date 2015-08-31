namespace KJFramework.Indexers
{
    /// <summary>
    ///     缓存索引器，提供了相关的基本属性结构
    /// </summary>
    internal class CacheIndexer : ICacheIndexer
    {
        #region Implementation of ICacheIndexer

        protected int _beginOffset;
        protected int _segmentSize;
        protected byte[] _cacheBuffer;

        /// <summary>
        ///     获取索引器开始的偏移量
        /// </summary>
        public int BeginOffset
        {
            get { return _beginOffset; }
            internal set { _beginOffset = value; }
        }

        /// <summary>
        ///     获取当前缓存数据段大小
        /// </summary>
        public int SegmentSize
        {
            get { return _segmentSize; }
            internal set { _segmentSize = value; }
        }

        /// <summary>
        ///     获取缓存缓冲区
        /// </summary>
        public byte[] CacheBuffer
        {
            get { return _cacheBuffer; }
            internal set { _cacheBuffer = value; }
        }

        #endregion
    }
}