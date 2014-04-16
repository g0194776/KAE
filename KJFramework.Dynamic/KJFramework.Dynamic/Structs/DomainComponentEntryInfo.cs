using System;

namespace KJFramework.Dynamic.Structs
{
    /// <summary>
    ///     程序域组件信息结构体
    /// </summary>
    public class DomainComponentEntryInfo
    {
        /// <summary>
        ///     获取或设置文件地址
        /// </summary>
        public String FilePath { get; set; }
        /// <summary>
        ///     获取或设置文件夹地址
        /// </summary>
        public String FolderPath { get; set; }
        /// <summary>
        ///     获取或设置入口点地址
        /// </summary>
        public String EntryPoint { get; set; }
        /// <summary>
        ///     获取或设置版本信息
        /// </summary>
        String Version { get; set; }
    }
}