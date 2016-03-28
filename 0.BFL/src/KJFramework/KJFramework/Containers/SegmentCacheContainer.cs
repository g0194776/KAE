using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using KJFramework.Cores;
using KJFramework.Cores.Segments;
using KJFramework.EventArgs;
using KJFramework.Exceptions;
using KJFramework.Indexers;
using KJFramework.Timer;
using KJFramework.Tracing;

namespace KJFramework.Containers
{
    /// <summary>
    ///     片段式缓存容器，提供了相关的基本操作
    /// </summary>
    /// <typeparam name="T">唯一标识类型</typeparam>
    public class SegmentCacheContainer<T> : ISegmentCacheContainer<T>
    {
        #region Constructor

        /// <summary>
        ///     片段式缓存容器，提供了相关的基本操作
        /// </summary>
        /// <param name="capacity">最大容量</param>
        /// <param name="policy">分段式缓存分布策略</param>
        public SegmentCacheContainer(int capacity, ISegmentCachePolicy policy)
        {
            if (policy == null)
            {
                throw new ArgumentNullException("policy");
            }
            if (capacity <=0)
            {
                throw new OutOfRangeException("Illegal capacity.");
            }
            _capacity = capacity;
            _policy = policy;
            _data = new byte[_capacity];
            Initialize();
        }

        #endregion

        #region Members

        private int _lastSize;
        private int[] _levels;
        private int _lastLevelSize;
        private readonly byte[] _data;
        private const int _timeoutInterval = 30000;
        private readonly ISegmentCachePolicy _policy;
        private readonly object _lockObj = new object();
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof (SegmentCacheContainer<T>));
        private Dictionary<int, ConcurrentStack<ISegmentCacheStub>> _pools;
        private readonly Dictionary<T, IHightSpeedSegmentCache> _caches = new Dictionary<T, IHightSpeedSegmentCache>();

        #endregion

        #region Methods

        /// <summary>
        ///     初始化
        /// </summary>
        private void Initialize()
        {
            if (_policy.SegmentLevel <= 0)
            {
                throw new OutOfRangeException("Cannot initialize a segment cache container, because the segment level must larger than zero.");
            }
            _levels = new int[_policy.SegmentLevel];
            _pools = new Dictionary<int, ConcurrentStack<ISegmentCacheStub>>(_policy.SegmentLevel);
            int offset = 0;
            int offsetLevel = 0;
            foreach (SegmentSizePair segmentSizePair in _policy.Get())
            {
                _levels[offsetLevel++] = segmentSizePair.Size;
                _pools.Add(segmentSizePair.Size, CreateStub(ref offset, segmentSizePair.Size, segmentSizePair.Percent));
            }
            Array.Sort(_levels);
            Array.Reverse(_levels);
            _lastLevelSize = _levels[_levels.Length - 1];
            //30s.
            LightTimer.NewTimer(_timeoutInterval, -1).Start(
                delegate
                    {
                        IList<T> values = new List<T>();
                        foreach (KeyValuePair<T, IHightSpeedSegmentCache> pair in _caches)
                        {
                            if (pair.Value.IsDead)
                            {
                                values.Add(pair.Key);
                            }
                        }
                        foreach (T key in values)
                        {
                            Remove(key);
                            T temp = key;
                            //use threadpool to notify expire event.
                            ThreadPool.QueueUserWorkItem(delegate { ExpiredHandler(new LightSingleArgEventArgs<T>(temp)); });
                        }
                    }, null);
        }

        /// <summary>
        ///     根据百分比创建缓存存根
        /// </summary>
        /// <param name="offset">内存段的偏移量</param>
        /// <param name="size">片段大小</param>
        /// <param name="percent">百分比</param>
        private ConcurrentStack<ISegmentCacheStub> CreateStub(ref int offset, int size, float percent)
        {
            int percentSize = (int) (_capacity*percent) + _lastSize;
            int cacheCount = percentSize/size;
            _lastSize = percentSize - (size*cacheCount);
            ConcurrentStack<ISegmentCacheStub> stack = new ConcurrentStack<ISegmentCacheStub>();
            for (int i = 0; i < cacheCount; i++)
            {
                ISegmentCacheStub stub = new SegmentCacheStub(new CacheIndexer
                                                                  {
                                                                      CacheBuffer = _data, 
                                                                      BeginOffset = offset, 
                                                                      SegmentSize = size
                                                                  });
                stack.Push(stub);
                offset += size;
            }
            return stack;
        }

        /// <summary>
        ///     获取指定大小缓存所需要的内存片段数量
        /// </summary>
        /// <param name="size">缓存大小</param>
        /// <returns>返回所需要的缓存数量</returns>
        private List<ISegmentCacheStub> GetStubs(int size)
        {
            int count;
            int lastSize;
            int currentLevel;
            List<ISegmentCacheStub> result = new List<ISegmentCacheStub>();
            try
            {
                int subSize;
                for (int i = 0; i < _levels.Length; i++)
                {
                    currentLevel = _levels[i];
                    //优先选择靠近当前等级的分块
                    if ((subSize = currentLevel - size) >= 0 && subSize <= _lastLevelSize)
                    {
                        ISegmentCacheStub segmentCacheStub;
                        if(!_pools[currentLevel].TryPop(out segmentCacheStub)) continue;
                        result.Add(segmentCacheStub);
                        return result;
                    }
                    if (size > currentLevel)
                    {
                        count = size / currentLevel;
                        lastSize = size - (currentLevel * count);
                        ISegmentCacheStub[] stubs;
                        ConcurrentStack<ISegmentCacheStub> stubStack = _pools[currentLevel];
                        //条件满足
                        if (stubStack.IsEmpty && i == _levels.Length - 1)
                        {
                            return null;
                        }
                        stubs = new SegmentCacheStub[count];
                        if (stubStack.TryPopRange(stubs, 0, count) != count)
                        {
                            //giveback now.
                            Giveback(stubs);
                            continue;
                        }
                        result.AddRange(stubs);
                        List<ISegmentCacheStub> temp = GetStubs(lastSize);
                        if (temp == null)
                        {
                            GivebackRang(result.ToArray());
                            return null;
                        }
                        result.AddRange(temp);
                        return result;
                    }
                }
                ISegmentCacheStub stub;
                if (!_pools[_levels[_levels.Length - 1]].TryPop(out stub)) return null;
                //小于所有的
                result.Add(stub);
                return result;
            }
            catch (OutOfMemoryException ex)
            {
                GivebackRang(result.ToArray());
                _tracing.Error(ex, null);
                return null;
            }
            catch(Exception ex)
            {
                _tracing.Error(ex, null);
                throw;
            }
        }

        /// <summary>
        ///     归还一个具有相同片段大小的缓存存根
        /// </summary>
        /// <param name="stubs">缓存存根集合</param>
        private void GivebackRang(ISegmentCacheStub[] stubs)
        {
            if (stubs.Length > 0)
            {
                int segmentSize = stubs[0].Indexer.SegmentSize;
                _pools[segmentSize].PushRange(stubs);
            }
        }

        /// <summary>
        ///     回归缓存存根
        /// </summary>
        /// <param name="segmentCacheStubs">缓存存根集合</param>
        private void Giveback(IList<ISegmentCacheStub> segmentCacheStubs)
        {
            //giveback now.
            foreach (ISegmentCacheStub stub in segmentCacheStubs)
                if (stub != null) _pools[stub.Indexer.SegmentSize].Push(stub);
        }

        /// <summary>
        ///     回归缓存存根
        /// </summary>
        /// <param name="segmentCacheStubs">缓存存根集合</param>
        private void Giveback(ISegmentCacheStub[] segmentCacheStubs)
        {
            //giveback now.
            foreach (ISegmentCacheStub stub in segmentCacheStubs)
            {
                if (stub != null)
                {
                    _pools[stub.Indexer.SegmentSize].Push(stub);
                }
            }
        }

        #endregion

        #region Implementation of ISegmentCacheContainer<T>

        private readonly int _capacity;

        /// <summary>
        ///     获取最大容量
        /// </summary>
        public int Capacity
        {
            get { return _capacity; }
        }

        /// <summary>
        ///     添加一个缓存
        /// </summary>
        /// <param name="key">缓存唯一标识</param>
        /// <param name="obj">缓存数据</param>
        /// <returns>返回添加后的标示</returns>
        public bool Add(T key, byte[] obj)
        {
            return Add(key, obj, DateTime.MaxValue);
        }

        /// <summary>
        ///     添加一个缓存
        /// </summary>
        /// <param name="key">缓存唯一标识</param>
        /// <param name="obj">缓存数据</param>
        /// <param name="timeSpan">续租时间</param>
        /// <returns>返回添加后的标示</returns>
        public bool Add(T key, byte[] obj, TimeSpan timeSpan)
        {
            return Add(key, obj, DateTime.Now.Add(timeSpan));
        }

        /// <summary>
        ///     添加一个缓存
        /// </summary>
        /// <param name="key">缓存唯一标识</param>
        /// <param name="obj">缓存数据</param>
        /// <param name="expireTime">过期时间</param>
        /// <returns>返回添加后的标示</returns>
        public bool Add(T key, byte[] obj, DateTime expireTime)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            List<ISegmentCacheStub> segmentCacheStubs = GetStubs(obj.Length);
            if (segmentCacheStubs == null) return false;
            int offset = 0;
            int lastSize = obj.Length;
            IHightSpeedSegmentCache hightSpeedCache = new HightSpeedSegmentCache(new CacheLease(expireTime), obj.Length);
            foreach (ISegmentCacheStub segmentCacheStub in segmentCacheStubs)
            {
                byte[] tempData = new byte[segmentCacheStub.Indexer.SegmentSize];
                int usedSize = lastSize > tempData.Length ? tempData.Length : lastSize;
                Buffer.BlockCopy(obj, offset, tempData, 0, usedSize);
                lastSize -= tempData.Length;
                offset += tempData.Length;
                //set data.
                ISegmentCacheItem item = (ISegmentCacheItem)segmentCacheStub.Cache;
                item.SetValue(tempData, usedSize);
                hightSpeedCache.Add(segmentCacheStub);
            }
            lock (_lockObj) _caches[key] = hightSpeedCache;
            return true;
        }

        /// <summary>
        ///     获取具有指定唯一标识的缓存数据
        /// </summary>
        /// <param name="key">缓存唯一标识</param>
        /// <returns>返回缓存数据</returns>
        public byte[] Get(T key)
        {
            lock (_lockObj)
            {
                IHightSpeedSegmentCache cache;
                if (_caches.TryGetValue(key, out cache))
                {
                    //already dead, waiting for giveback.
                    return cache.IsDead ? null : cache.GetBody();
                }
                return null;
            }
        }

        /// <summary>
        ///     检查当前是否存在具有指定唯一标识的缓存
        /// </summary>
        /// <param name="key">缓存唯一标识</param>
        /// <returns>返回是否存在的标识</returns>
        public bool IsExists(T key)
        {
            lock (_lockObj) return _caches.ContainsKey(key);
        }

        /// <summary>
        ///     移除具有指定唯一标示的缓存
        /// </summary>
        /// <param name="key">缓存唯一标识</param>
        public void Remove(T key)
        {
            lock (_lockObj)
            {
                IHightSpeedSegmentCache cache;
                if (!_caches.TryGetValue(key, out cache)) return;
                _caches.Remove(key);
                IList<ISegmentCacheStub> segmentCacheStubs = cache.GetStubs();
                Giveback(segmentCacheStubs);
            }
        }

        /// <summary>
        ///     过期事件
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<T>> Expired;
        protected void ExpiredHandler(LightSingleArgEventArgs<T> e)
        {
            EventHandler<LightSingleArgEventArgs<T>> handler = Expired;
            if (handler != null) handler(this, e);
        }

        #endregion
    }
}