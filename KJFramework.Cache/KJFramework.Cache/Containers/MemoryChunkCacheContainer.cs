using System;
using System.Collections.Concurrent;
using System.Runtime;
using System.Web;
using KJFramework.Cache.Objects;

namespace KJFramework.Cache.Containers
{
    /// <summary>
    ///     连续内存段缓存容器，提供了相关的基本操作
    /// </summary>
    public class MemoryChunkCacheContainer : IMemoryChunkCacheContainer
    {
        #region Constructor

        /// <summary>
        ///     连续内存段缓存容器，提供了相关的基本操作
        /// </summary>
        /// <param name="segmentSize">内存片段大小</param>
        /// <param name="memoryChunkSize">
        ///     连续内存段大小
        ///     <para>* 单位：byte</para>
        ///     <para>* 该参数应该是segmentSize的倍数</para>
        /// </param>
        public MemoryChunkCacheContainer(int segmentSize, int memoryChunkSize)
        {
            if (segmentSize <= 0) throw new ArgumentException("Illegal segment size. #" + segmentSize);
            if (memoryChunkSize <= 0) throw new ArgumentException("Illegal memory chunk size. #" + memoryChunkSize);
            if (memoryChunkSize < segmentSize) throw new ArgumentException("Illegal memory chunk size, because current memory chunk size must > segment size. #" + memoryChunkSize);
            _segmentSize = segmentSize;
            _memoryChunkSize = memoryChunkSize;
            #region Memory Fail Point Check.
            
            #if(!MONO_ENV)
            {
                //There is a BUG when your .NETFRAMEWORK version is .NET 4.5 and host enviroment is IIS
                //MemoryFailPoint wil' not be work correctly...
                //only execution host.
                if (HttpContext.Current == null)
                {
                    int totalMemory = (memoryChunkSize / 1024) / 1024;
                    new MemoryFailPoint(totalMemory > 0 ? totalMemory : 1);
                }

            }
            #endif

            #endregion
            _data = new byte[_memoryChunkSize];
            //try to initialize memory segment.
            Initialize();
        }

        #endregion

        #region Implementation of IMemoryChunkCacheContainer

        private readonly int _segmentSize;
        private readonly int _memoryChunkSize;
        private readonly byte[] _data;
        private readonly ConcurrentStack<IMemorySegment> _segments = new ConcurrentStack<IMemorySegment>();

        /// <summary>
        ///     获取内存片段大小
        /// </summary>
        public int SegmentSize
        {
            get { return _segmentSize; }
        }

        /// <summary>
        ///     获取连续占用的内存段大小
        /// </summary>
        public int MemoryChunkSize
        {
            get { return _memoryChunkSize; }
        }

        /// <summary>
        ///     租借一个内存片段
        /// </summary>
        /// <returns>如果返回null, 则证明已经没有剩余的内存片段可以被租借</returns>
        public IMemorySegment Rent()
        {
            IMemorySegment segment;
            return _segments.TryPop(out segment) ? segment : null;
        }

        /// <summary>
        ///     归还一个内存片段
        /// </summary>
        /// <param name="segment">内存片段</param>
        public void Giveback(IMemorySegment segment)
        {
            if (segment == null) throw new ArgumentNullException("segment");
            segment.UsedBytes = 0;
            _segments.Push(segment);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     初始化内存片段集合
        /// </summary>
        private void Initialize()
        {
            int remainingSize = _memoryChunkSize;
            int offset = 0;
            do
            {
                IMemorySegment segment = new MemorySegment(new ArraySegment<byte>(_data, offset, remainingSize > _segmentSize ? _segmentSize : remainingSize));
                offset += _segmentSize;
                remainingSize -= _segmentSize;
                _segments.Push(segment);
            } while (remainingSize > 0);
        }

        #endregion
    }
}