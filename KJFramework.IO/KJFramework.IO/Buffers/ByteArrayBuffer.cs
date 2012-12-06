using System;
using System.Collections.Generic;

namespace KJFramework.IO.Buffers
{
    /// <summary>
    ///   字节数组缓冲区，更多用于解析消息包时缓冲接收到的字节数组
    /// </summary>
    public abstract class ByteArrayBuffer : IByteArrayBuffer
    {
        private object oo = new object();

        #region 构造函数

        /// <summary>
        ///   字节数组缓冲区，更多用于解析消息包时缓冲接收到的字节数组
        ///   <para>* 缓冲区的大小应该设置为：缓冲区长度 * 容量。</para>
        /// </summary>
        /// <param name="bufferSize">缓冲区长度</param>
        /// <param name="capacity">容量</param>
        public ByteArrayBuffer(int bufferSize)
        {
            if (bufferSize <= 0)
            {
                throw new System.Exception("无法初始化缓冲区，非法的缓冲区长度");
            }
            _buffer = new byte[bufferSize];
        }

        #endregion

        #region Members

        protected int _offset;
        protected byte[] _buffer;

        #endregion

        #region Implementation of IByteArrayBuffer

        protected int _bufferSize;
        protected bool _autoClear;

        /// <summary>
        ///   获取缓冲区大小
        ///   <para>* 缓冲区的大小应该设置为：缓冲区长度 * 容量。</para>
        /// </summary>
        public int BufferSize
        {
            get { return _bufferSize; }
        }

        /// <summary>
        ///   添加缓存
        /// </summary>
        /// <param name="data">接收到的数据</param>
        public List<byte[]> Add(byte[] data)
        {
            lock (oo)
            {
                //如果当前缓冲区剩余的空间已经无法承载下接受数据的长度。
                if (_buffer.Length - _offset < data.Length)
                {
                    //自动重置缓冲区
                    if (_autoClear) Clear();
                    else throw new System.Exception("缓冲区无法再接收新的数据，因为缓冲区已经满了。请尝试设置AutoClear = true来解决此问题。");
                }
                //复制数据
                Buffer.BlockCopy(data, 0, _buffer, _offset, data.Length);
                _offset += data.Length;
                //提取数据
                int currentOffset = 0;
                List<byte[]> result = PickupData(ref currentOffset, _offset);
                //没有提取到有用数据
                if (currentOffset <= 0) return null;
                //提取到了有用数据
                if (currentOffset > 0)
                {
                    int leaveSize = _buffer.Length - currentOffset;
                    //如果当前有用的内容长度，正好等于缓冲区总长度，则重置缓冲区偏移量
                    if (leaveSize <= 0)
                    {
                        //这里设置偏移量为0，而不重新初始化缓冲区的目的是为了减少内存操作
                        //下一次的操作将直接复写0位的数据
                        _offset = 0;
                        return result;
                    }
                    //复制剩余的数据
                    byte[] lastData = new byte[leaveSize];
                    Buffer.BlockCopy(_buffer, currentOffset, lastData, 0, lastData.Length);
                    //复制到原本的缓冲区中
                    Buffer.BlockCopy(lastData, 0, _buffer, 0, lastData.Length);
                    //重置偏移量
                    _offset -= currentOffset;
                    return result;
                }
                return null;
            }
        }

        /// <summary>
        ///   清空缓冲区
        /// </summary>
        public void Clear()
        {
            _buffer.Initialize();
            _offset = 0;
        }

        /// <summary>
        ///   获取或设置一个值，改值标示了如果缓冲区不够的时候，是否自动重置缓冲区
        /// </summary>
        public bool AutoClear
        {
            get { return _autoClear; }
            set { _autoClear = value; }
        }

        #endregion

        #region Functions

        /// <summary>
        ///   第三方用户使用的方法，意在使用自己的方式提取有用的数据
        /// </summary>
        /// <returns></returns>
        protected abstract List<byte[]> PickupData(ref int nextOffset, int offset);

        #endregion
    }
}