namespace KJFramework.Messages.Enums
{
    /// <summary>
    ///     使用的压缩方式枚举
    /// </summary>
    public enum CompressionTypes : byte
    {
        /// <summary>
        ///     GZip 压缩格式
        /// </summary>
        GZip = 0x00,
        /// <summary>
        ///     BZip2 压缩格式
        /// </summary>
        BZip2 = 0x01
    }
}