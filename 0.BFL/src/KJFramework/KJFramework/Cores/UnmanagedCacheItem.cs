using System;
using System.Runtime.InteropServices;

namespace KJFramework.Cores
{
    /// <summary>
    ///     非托管缓存项，提供了相关的基本操作。
    /// </summary>
    internal class UnmanagedCacheItem : IUnmanagedCacheItem
    {
        #region Constructor

        /// <summary>
        ///     非托管缓存项，提供了相关的基本操作。
        /// </summary>
        /// <param name="size">需要申请的内存大小</param>
        public UnmanagedCacheItem(int size)
        {
            _size = size;
            if (size <= 0)
            {
                throw new ArgumentException("Illegal unmanaged memory. #size: " + size);
            }
            _ptr = Marshal.AllocHGlobal(size);
        }

        /// <summary>
        ///     非托管缓存项，提供了相关的基本操作。
        /// </summary>
        /// <param name="ptr">内存句柄</param>
        /// <param name="size">需要申请的内存大小</param>
        /// <param name="useageSize">已使用长度</param>
        public UnmanagedCacheItem(IntPtr ptr, int size, int useageSize)
        {
            if (ptr == IntPtr.Zero)
            {
                throw new ArgumentException("Memory handle can not be zero.");
            }
            _size = size;
            _currentUsageSize = useageSize == 0 ? -1 : useageSize;
            _ptr = ptr;
        }

        #endregion

        #region Members

        protected readonly int _size;
        protected IntPtr _ptr = IntPtr.Zero;
        protected int _currentUsageSize = -1;

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
            if (_currentUsageSize == -1)
            {
                data = new byte[_size];
                Marshal.Copy(_ptr, data, 0, _size);
                return data;
            }
            data = new byte[_currentUsageSize];
            Marshal.Copy(_ptr, data, 0, _currentUsageSize);
            return data;
        }

        /// <summary>
        ///     设置缓存内容
        /// </summary>
        /// <param name="obj">缓存对象</param>
        public void SetValue(byte[] obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            if (obj.Length > _size)
            {
                throw new ArgumentException("Out of range size. #size: " + obj.Length);
            }
            _currentUsageSize = obj.Length;
            Marshal.Copy(obj, 0, _ptr, obj.Length);
        }

        /// <summary>
        ///     获取内存句柄
        /// </summary>
        public IntPtr Handle
        {
            get { return _ptr; }
        }

        /// <summary>
        ///     获取当前的使用大小
        /// </summary>
        public int UsageSize
        {
            get { return _currentUsageSize; }
        }

        /// <summary>
        ///     获取当前的最大容量
        /// </summary>
        public int MaxSize
        {
            get { return _size; }
        }

        /// <summary>
        ///     释放当前所持有的非托管缓存
        /// </summary>
        public void Free()
        {
            if (_ptr == IntPtr.Zero) return;
            Marshal.FreeHGlobal(_ptr);
            _ptr = IntPtr.Zero;
        }

        #endregion
    }
}