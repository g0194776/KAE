using KJFramework.Dynamic.Structs;

namespace KJFramework.ApplicationEngine.Objects
{
    /// <summary>
    ///     应用入口信息对象
    /// </summary>
    public class ApplicationEntryInfo : DomainComponentEntryInfo
    {
        #region Members.

        /// <summary>
        ///     获取或设置文件CRC值
        /// </summary>
        public long FileCRC { get; set; }

        #endregion
    }
}