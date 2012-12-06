using System;
using System.Diagnostics;

namespace KJFramework.Cache.Objects
{
    /// <summary>
    ///     字节数组内存片段，提供了相关的基本操作
    /// </summary>
    [DebuggerDisplay("Usedbytes: {UsedBytes}, Offset: {Segment.Offset}")]
    public class MemorySegment : IMemorySegment
    {
        #region Constructor

        /// <summary>
        ///     字节数组内存片段，提供了相关的基本操作
        /// </summary>
        /// <param name="segment">字节数组内存片段</param>
        public MemorySegment(ArraySegment<byte> segment)
        {
            _segment = segment;
        }

        #endregion

        #region Implementation of IMemorySegment

        private readonly ArraySegment<byte> _segment;
        private int _usedBytes;

        /// <summary>
        ///     获取内部的字节数组片段
        /// </summary>
        public ArraySegment<byte> Segment
        {
            get { return _segment; }
        }

        /// <summary>
        ///     获取或设置已使用的字节数量
        /// </summary>
        public int UsedBytes
        {
            get { return _usedBytes; }
            set { _usedBytes = value; }
        }

        /// <summary>
        ///     获取或设置已使用偏移量
        ///     <para>* 我们建议您应该总是使用此属性来判断当前可用数据的真实偏移量.</para>
        ///     <para>* 当设置此属性后，我们将会自动计算UsedBytes.</para>
        /// </summary>
        public int UsedOffset
        {
            get { return _segment.Offset + _usedBytes; }
            set { _usedBytes += value; }
        }

        #endregion
    }
}