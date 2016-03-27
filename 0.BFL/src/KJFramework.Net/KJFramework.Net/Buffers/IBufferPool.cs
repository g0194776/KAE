namespace KJFramework.Net.Buffers
{
    /// <summary>
    ///     缓冲区元接口, 提供了相关的基本操作
    /// </summary>
    public interface IBufferPool
    {
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
        void Write(byte[] data, int offset, int length);
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
        int Read(byte[] data, int offset, int length);
        /// <summary>
        ///     获取或设置缓冲池储存上限大小。
        ///         * 如果超出该大小，将会自动清空缓冲池。
        /// </summary>
        int DiscardSize { get; set; }
        /// <summary>
        ///     清空缓冲区
        /// </summary>
        void Clear();
        /// <summary>
        ///     获取缓冲区长度 : 数据类型 - long
        /// </summary>
        /// <returns>
        ///     获取的长度只是当前缓冲区存在的内容长度，总长度不算在内。
        /// </returns>
        long GetLength();
    }
}
