using System.Collections.Generic;

namespace KJFramework.Buffers
{
    /// <summary>
    ///   字节数组缓冲区，更多用于解析消息包时缓冲接收到的字节数组
    /// </summary>
    public interface IByteArrayBuffer
    {
        /// <summary>
        ///   获取缓冲区大小
        ///   <para>* 缓冲区的大小应该设置为：缓冲区长度 * 容量。</para>
        /// </summary>
        int BufferSize { get; }
        /// <summary>
        ///   添加缓存
        /// </summary>
        /// <param name="data">接收到的数据</param>
        /// <returns>返回提取后的数据</returns>
        List<byte[]> Add(byte[] data);
        /// <summary>
        ///   清空缓冲区
        /// </summary>
        void Clear();
        /// <summary>
        ///   获取或设置一个值，改值标示了如果缓冲区不够的时候，是否自动重置缓冲区
        /// </summary>
        bool AutoClear { get; set; }
    }
}