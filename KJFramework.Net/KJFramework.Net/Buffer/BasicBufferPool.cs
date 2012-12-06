using System;
using System.IO;
using KJFramework.Logger;
using KJFramework.Logger.LogObject;
using KJFramework.Net.Helper;

namespace KJFramework.Net.Buffer
{
    /// <summary>
    ///     提供了基本的数据缓冲池的相关结构, 以及相关基本的操作
    /// </summary>
    /// <remarks>
    ///     此缓冲池用来存放片段数据，或者粘包造成数据溢出后的数据。
    /// </remarks>
    public class BasicBufferPool : IBufferPool
    {
        /// <summary>
        ///     内部内存流缓存
        /// </summary>
        protected MemoryStream _buffer = new MemoryStream();

        /// <summary>
        ///     提供了基本的数据缓冲池的相关结构, 以及相关基本的操作
        /// </summary>
        /// <remarks>
        ///     此缓冲池用来存放片段数据，或者粘包造成数据溢出后的数据。
        /// </remarks>
        public BasicBufferPool() : this(NetHelper.DefaultBufferPoolDiscardSize)
        {
        }

        /// <summary>
        ///     提供了基本的数据缓冲池的相关结构, 以及相关基本的操作
        /// </summary>
        /// <param name="discardSize">缓冲池储存上限大小</param>
        /// <remarks>
        ///     此缓冲池用来存放片段数据，或者粘包造成数据溢出后的数据。
        /// </remarks>
        public BasicBufferPool(int discardSize)
        {
            if (discardSize < 0 || discardSize > 100000)
            {
                throw new SystemException("缓冲池大小设置非法。");
            }
            _discardSize = discardSize;
        }

        #region IBufferPool 成员

        private IDebugLogger<IDebugLog> _debuglogger;

        /// <summary>
        ///     获取或设置异常记录器
        /// </summary>
        public IDebugLogger<IDebugLog> DebugLogger
        {
            get
            {
                return _debuglogger;
            }
            set
            {
                _debuglogger = value;
            }
        }

        /// <summary>
        ///     向缓冲区写入指定数据
        /// </summary>
        /// <param name="data" type="byte[]">
        ///     <para>
        ///         写入的数据 : byte[]
        ///     </para>
        /// </param>
        /// <param name="offset" type="int">
        ///     <para>
        ///         写入偏移 : int
        ///     </para>
        /// </param>
        /// <param name="length" type="int">
        ///     <para>
        ///         写入长度 : int
        ///     </para>
        /// </param>
        public void Write(byte[] data, int offset, int length)
        {
            if (_buffer.Length >= _discardSize || _buffer.Length + data.Length >= _discardSize)
            {
                Clear();
            }
            _buffer.Write(data, offset, length);
        }

        /// <summary>
        ///     从指定偏移处读取缓冲区指定长度到字节数组
        /// </summary>
        /// <param name="data" type="byte[]">
        ///     <para>
        ///         要读取到的数据集合 : byte[] - Ref
        ///     </para>
        /// </param>
        /// <param name="offset" type="int">
        ///     <para>
        ///         读取偏移 : int
        ///     </para>
        /// </param>
        /// <param name="length" type="int">
        ///     <para>
        ///         读取长度 : int
        ///     </para>
        /// </param>
        /// <returns>
        ///     返回读取长度
        /// </returns>
        public int Read(byte[] data, int offset, int length)
        {
            return _buffer.Read(data, offset, length);
        }

        /// <summary>
        ///     清空缓冲区
        /// </summary>
        public void Clear()
        {
            _buffer.Close();
            _buffer = new MemoryStream();
        }

        /// <summary>
        ///     获取缓冲区长度 : 数据类型 - long
        /// </summary>
        /// <returns>
        ///     获取的长度只是当前缓冲区存在的内容长度，总长度不算在内。
        /// </returns>
        public long GetLength()
        {
            return _buffer.Length;
        }

        private int _discardSize;

        /// <summary>
        ///     获取或设置缓冲池储存上限大小。
        ///         * 如果超出该大小，将会自动清空缓冲池。
        /// </summary>
        public int DiscardSize
        {
            get
            {
                return _discardSize;
            }
            set
            {
                _discardSize = value;
            }
        }

        #endregion
    }
}
