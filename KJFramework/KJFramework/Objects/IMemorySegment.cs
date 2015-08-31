using System;

namespace KJFramework.Objects
{
    /// <summary>
    ///     字节数组内存片段元接口，提供了相关的基本操作
    /// </summary>
    public interface IMemorySegment
    {
        /// <summary>
        ///     获取内部的字节数组片段
        /// </summary>
        ArraySegment<byte> Segment { get; }
        /// <summary>
        ///     获取或设置已使用的字节数量
        /// </summary>
        int UsedBytes { get; set; }
        /// <summary>
        ///     获取或设置已使用偏移量
        ///     <para>* 我们建议您应该总是使用此属性来判断当前可用数据的真实偏移量.</para>
        ///     <para>* 当设置此属性后，我们将会自动计算UsedBytes.</para>
        /// </summary>
        int UsedOffset { get; set; }
    }
}