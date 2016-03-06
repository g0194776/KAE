﻿using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using KJFramework.Cache.Cores;
using KJFramework.PerformanceProvider;
using KJFramework.Tracing;

namespace KJFramework.Cache.Containers
{
    /// <summary>
    ///     固态的缓存容器，提供了相关的基本操作
    /// </summary>
    /// <typeparam name="T">缓存对象类型</typeparam>
    internal class FixedCacheContainer<T> : IFixedCacheContainer<T>
        where T : IClearable, new()
    {
        #region Constructor

        /// <summary>
        ///     固态的缓存容器，提供了相关的基本操作
        /// </summary>
        /// <param name="capacity">最大容量</param>
        public FixedCacheContainer(int capacity)
        {
            if (capacity <= 0)
            {
                throw new ArgumentException("Illegal capacity!");
            }
            _remainingCount = _capacity = capacity;
            _caches = new ConcurrentStack<ICacheStub<T>>();
            //initialize.
            for (int i = 0; i < _capacity; i++)
            {
                ICacheStub<T> stub = new CacheStub<T> { Fixed = true };
                stub.Cache = new CacheItem<T>();
                stub.Cache.SetValue(new T());
                _caches.Push(stub);
            }
        }

        #endregion

        #region Members

        private long _remainingCount;
        private PerfCounter _counter = null;
        private ConcurrentStack<ICacheStub<T>> _caches;
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof (FixedCacheContainer<T>));
        private static readonly string _perfCounterCategory = "#FIX-Cache Container. - " + Process.GetCurrentProcess().ProcessName;

        #endregion

        #region Implementation of IFixedCacheContainer<T>

        private readonly int _capacity;

        /// <summary>
        ///     获取当前容器的最大容量
        /// </summary>
        public int Capacity
        {
            get { return _capacity; }
        }

        /// <summary>
        ///     租借一个缓存
        /// </summary>
        /// <returns>返回一个新的缓存</returns>
        public IFixedCacheStub<T> Rent()
        {
            ICacheStub<T> cache;
            if (!_caches.IsEmpty && _caches.TryPop(out cache))
            {
                Interlocked.Decrement(ref _remainingCount);
                if (_counter != null) _counter.Decrement();
                return (IFixedCacheStub<T>)cache;
            }
            return null;
        }

        /// <summary>
        ///     归还一个缓存
        /// </summary>
        /// <param name="cache">缓存</param>
        public void Giveback(IFixedCacheStub<T> cache)
        {
            if (cache == null) throw new ArgumentNullException("cache");
            if (Interlocked.Read(ref _remainingCount) >= _capacity) throw new System.Exception("Can not giveback a cache, because target caches count has been full.");
            cache.Cache.Clear();
            cache.Tag = null;
            _caches.Push((ICacheStub<T>)cache);
            Interlocked.Increment(ref _remainingCount);
            if (_counter != null) _counter.Increment();
        }

        /// <summary>
        ///    构造内部性能计数器
        /// </summary>
        /// <param name="name">性能计数器名称</param>
        public void BuildPerformanceCounter(string name)
        {
            if (string.IsNullOrEmpty(name)) return;
            try
            {
                #region Ensure Performance Counter category exist.

                CounterCreationDataCollection dataCollection = new CounterCreationDataCollection();
                if (PerformanceCounterCategory.Exists(_perfCounterCategory)) PerformanceCounterCategory.Delete(_perfCounterCategory);
                CounterCreationData data = new CounterCreationData(name, "This was automic created by KJFramework. It'll be used for the infomation collections.", PerformanceCounterType.NumberOfItems64);
                //add default performance counter for each processor.
                dataCollection.Add(data);
                PerformanceCounterCategory.Create(_perfCounterCategory, string.Format("#This was automic created by KJFramework: {0}, Pls *DO NOT* remove it by manual.", Process.GetCurrentProcess().ProcessName), PerformanceCounterCategoryType.MultiInstance, dataCollection);

                #endregion
                _counter = new PerfCounter(_perfCounterCategory, Process.GetCurrentProcess().ProcessName, new PerfCounterAttribute(name, PerformanceCounterType.NumberOfItems64));
                _counter.IncrementBy(_capacity);
            }
            catch (System.Exception ex) { _tracing.Error(ex, null); }
        }

        #endregion

        #region Helpful Intenral Methods.

        /// <summary>
        ///    获取内部容器所包含的真实元素个数
        /// </summary>
        /// <returns>内部容器所包含的真实元素个数</returns>
        internal int GetCount()
        {
            return _caches.Count;
        }

        #endregion
    }
}