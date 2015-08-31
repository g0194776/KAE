using System;
using KJFramework.Indexers;

namespace KJFramework.Cores
{
    /// <summary>
    ///     内存段缓存项，提供了相关的基本操作
    /// </summary>
    internal class SegmentCacheItem : ISegmentCacheItem
    {
        #region Constructor

        /// <summary>
        ///     内存段缓存项，提供了相关的基本操作
        /// </summary>
        /// <param name="indexer">缓存索引器</param>
        public SegmentCacheItem(ICacheIndexer indexer)
        {
            if (indexer == null)
            {
                throw new ArgumentNullException("indexer");
            }
            _indexer = indexer;
        }

        #endregion

        #region Members

        protected readonly ICacheIndexer _indexer;
        protected int _usageSize = -1;
        protected bool _isUsed;

        #endregion

        #region Implementation of ICacheItem<byte[]>

        /// <summary>
        ///     获取缓存内容
        /// </summary>
        /// <returns>返回缓存内容</returns>
        public byte[] GetValue()
        {
            byte[] data;
            //all of the data.
            if (_usageSize == -1)
            {
                data = new byte[_indexer.SegmentSize];
                Buffer.BlockCopy(_indexer.CacheBuffer, _indexer.BeginOffset, data, 0, _indexer.SegmentSize);
                return data;
            }
            data = new byte[_usageSize];
            Buffer.BlockCopy(_indexer.CacheBuffer, _indexer.BeginOffset, data, 0, _usageSize);
            return data;
        }

        /// <summary>
        ///     设置缓存内容
        /// </summary>
        /// <param name="obj">缓存对象</param>
        public void SetValue(byte[] obj)
        {
            SetValue(obj, obj.Length);
        }

        /// <summary>
        ///     获取一个值，该值表示了当前的缓存是否已经被使用
        /// </summary>
        public bool IsUsed
        {
            get { return _isUsed; }
        }

        /// <summary>
        ///     获取当前的使用大小
        /// </summary>
        public int UsageSize
        {
            get { return _usageSize; }
        }

        /// <summary>
        ///     初始化
        /// </summary>
        public void Initialize()
        {
            _isUsed = false;
            byte[] data = new byte[_indexer.SegmentSize];
            Buffer.BlockCopy(data, 0, _indexer.CacheBuffer, _indexer.BeginOffset, data.Length);
        }

        /// <summary>
        ///     设置缓存内容
        /// </summary>
        /// <param name="obj">缓存对象</param>
        /// <param name="usedSize">使用大小</param>
        public void SetValue(byte[] obj, int usedSize)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            if (obj.Length > _indexer.SegmentSize)
            {
                throw new ArgumentException("Out of range size. #size: " + obj.Length);
            }
            _isUsed = true;
            _usageSize = usedSize;
            Buffer.BlockCopy(obj, 0, _indexer.CacheBuffer, _indexer.BeginOffset, usedSize);
        }

        #endregion
    }
}