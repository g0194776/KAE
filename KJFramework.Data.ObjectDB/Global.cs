using KJFramework.Data.ObjectDB.Structures;

namespace KJFramework.Data.ObjectDB
{
    /// <summary>
    ///      全局变量
    /// </summary>
    internal unsafe static class Global
    {
        #region Members

        /// <summary>
        ///     默认的类型令牌定义
        /// </summary>
        public static readonly TypeToken DefaultToken = new TypeToken();
        /// <summary>
        ///     每个页面中的数据片段数量
        /// </summary>
        public const ushort SegmentsPerPage = 8;
        /// <summary>
        ///     服务器设备的单页面大小
        ///     <para>* 单位: byte</para>
        /// </summary>
        public static readonly uint ServerPageSize = 32 * 1024 + (uint)sizeof(PageHead);
        /// <summary>
        ///     文件头边界长度
        /// </summary>
        public const uint HeaderBoundary = 1024*100;
        /// <summary>
        ///     最大单一根数据类型的存储量
        ///     <para>* 这里的数量代表了最多能跨越多少个文件进行存储</para>
        /// </summary>
        public const byte MaxDataStoreVolume = 10;
        /// <summary>
        ///     每个数据片段的大小
        ///     <para>* 单位: byte</para>
        /// </summary>
        public static readonly uint SegmentSize = ServerPageSize / SegmentsPerPage;
        /// <summary>
        ///     服务器设备页面数量
        /// </summary>
        public const uint ServerPageCount = 65535;
        /// <summary>
        ///     服务器设备单存储文件最大长度
        /// </summary>
        public const ulong MaxServerFileSize = 4294967296;
        /// <summary>
        ///     获取文件头
        /// </summary>
        public static readonly byte[] FileHeadFlag = new byte[] {0xFF, 0XFF, 0XFF, 0XFF};

        #endregion
    }
}