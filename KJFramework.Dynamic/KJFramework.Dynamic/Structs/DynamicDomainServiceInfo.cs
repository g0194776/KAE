using System;
using KJFramework.Dynamic.Components;

namespace KJFramework.Dynamic.Structs
{
    /// <summary>
    ///     动态程序域服务信息对象
    /// </summary>
    public struct DynamicDomainServiceInfo
    {
        /// <summary>
        ///     获取或设置动态程序域服务
        /// </summary>
        public IDynamicDomainService Service { get; set; }
        /// <summary>
        ///     获取或设置文件地址
        /// </summary>
        public String FilePath { get; set; }
        /// <summary>
        ///     获取或设置目录信息
        /// </summary>
        public String DirectoryPath { get; set; }
    }
}