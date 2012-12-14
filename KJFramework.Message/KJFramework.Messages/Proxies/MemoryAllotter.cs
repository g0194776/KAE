using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using KJFramework.Logger;
using KJFramework.Tracing;

namespace KJFramework.Messages.Proxies
{
    /// <summary>
    ///     内存申请器
    /// </summary>
    public unsafe class MemoryAllotter
    {
        #region Constructor

        /// <summary>
        ///     内存申请器
        /// </summary>
        private MemoryAllotter()
        {
            
        }

        #endregion

        #region Members

        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof (MemoryAllotter));
        private Stack<IMemorySegment> _segments = new Stack<IMemorySegment>();
        private object _lockObj = new object();
        private bool _initialized;
        private byte* _data;

        /// <summary>
        ///     内存片段大小
        ///     <para>* 取值范围: 64~1024，并且是64的倍数.</para>
        ///     <para>* 单位: byte.</para>
        /// </summary>
        public static uint SegmentSize = 256;
        /// <summary>
        ///     倍数
        ///     <para>* 当前申请的非托管内存大小 = Multiples*SegmentSize</para>
        /// </summary>
        public static uint Multiples = 100000;
        /// <summary>
        ///     获取或设置一个值，该值标示了是否允许在框架初始化的时候输出一些内部信息
        /// </summary>
        public static bool AllowPrintInfo = true;
        /// <summary>
        ///     获取或设置一个值，该值标示了当前框架的实体类解析操作是否支持兼容模式
        /// </summary>
        public static bool AllowCompatibleMode = true;
        /// <summary>
        ///     内存申请器
        /// </summary>
        public static readonly MemoryAllotter Instance = new MemoryAllotter();

        #endregion

        #region Members

        /// <summary>
        ///     初始化内存申请器
        /// </summary>
        /// <exception cref="ArgumentException">内部成员变量值不符合要求</exception>
        public void Initialize()
        {
            if (_initialized) return;
            if (SegmentSize > 1024 || SegmentSize < 64) throw new ArgumentException("Value range: 64~1024.");
            if (SegmentSize % 64 != 0) throw new ArgumentException("Value *MUST* be 64 multiples!");
            //max 10mb.
            int totalSize = (int) (SegmentSize * Multiples);
            _data = (byte*) Marshal.AllocHGlobal(totalSize);
            //create segments.
            lock (_lockObj)
            {
                for (int i = 0; i < Multiples; i++)
                    _segments.Push(new MemorySegment(_data, (uint)(i * SegmentSize), SegmentSize));
                GC.Collect();
            }
            _initialized = true;
            if(AllowPrintInfo)
                _tracing.Info(string.Format("\r\n[KJFramework.Message Unmanaged Memory Info]\r\n#Unmanaged Memory: {0} bytes.\r\n#Segments: {1}.", totalSize, Multiples));
        }

        /// <summary>
        ///     租借一个新的内存段
        /// </summary>
        /// <returns>返回租借后的内存段</returns>
        public IMemorySegment Rent()
        {
            lock(_lockObj) return _segments.Pop();
        }

        /// <summary>
        ///     租借一个新的内存段
        /// </summary>
        /// <returns>返回租借后的内存段</returns>
        public void Giveback(IMemorySegment segment)
        {
            segment.Initialize();
            lock (_lockObj) _segments.Push(segment);
        }

        #endregion
    }
}