using System;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Platform.Deploy.Metadata.Objects
{
    /// <summary>
    ///     文件详细信息对象
    /// </summary>
    public class FileInfo : IntellectObject
    {
        /// <summary>
        ///     获取或设置文件目录结构
        /// </summary>
        [IntellectProperty(0, IsRequire = false)]
        public string Directory { get; set; }
        /// <summary>
        ///     获取或设置文件名称
        /// </summary>
        [IntellectProperty(1, IsRequire = true)]
        public string FileName { get; set; }
        /// <summary>
        ///     获取或设置文件最后修改时间
        /// </summary>
        [IntellectProperty(2, IsRequire = false)]
        public DateTime LastModifyTime { get; set; }
        /// <summary>
        ///     获取或设置文件创建时间
        /// </summary>
        [IntellectProperty(3, IsRequire = false)]
        public DateTime CreateTime { get; set; }
        /// <summary>
        ///     获取或设置文件大小
        /// </summary>
        [IntellectProperty(4, IsRequire = true)]
        public double Size { get; set; }
        /// <summary>
        ///     获取或设置一个值，该值标示了当前文件是否存在
        /// </summary>
        [IntellectProperty(5, IsRequire = true)]
        public bool IsExists { get; set; }
    }
}