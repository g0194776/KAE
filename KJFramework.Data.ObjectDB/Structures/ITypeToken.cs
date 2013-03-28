namespace KJFramework.Data.ObjectDB.Structures
{
    /// <summary>
    ///     类型令牌接口
    /// </summary>
    internal interface ITypeToken
    {
        /// <summary>
        ///     获取当前类型令牌的编号
        /// </summary>
        ulong Id { get; }
        /// <summary>
        ///     获取或设置类型所使用的起始页编号
        /// </summary>
        ushort StartPageId { get; set; }
        /// <summary>
        ///     获取或设置类型所占用的页数目
        /// </summary>
        ushort PageCounts { get; set; }
        /// <summary>
        ///     获取或设置类型所使用的的起始文件编号
        /// </summary>
        ushort StartFileId { get; set; }
        /// <summary>
        ///     获取或设置类型所占用的文件数目
        /// </summary>
        ushort FileCounts { get; set; }
    }
}