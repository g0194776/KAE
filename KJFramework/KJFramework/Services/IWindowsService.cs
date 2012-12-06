using System;

namespace KJFramework.Services
{
    /// <summary>
    ///     WINDOWS服务元接口，提供了相关的基本操作。
    /// </summary>
    public interface IWindowsService : IDisposable
    {
        /// <summary>
        ///     开启服务
        /// </summary>
        /// <returns>返回开启的状态</returns>
        bool Start();
        /// <summary>
        ///     停止服务
        /// </summary>
        /// <returns>返回停止的状态</returns>
        bool Stop();
        /// <summary>
        ///     获取或设置服务名称
        /// </summary>
        String ServiceName { get; set; }
        /// <summary>
        ///     获取或设置显示名称
        /// </summary>
        String DisplayName { get; set; }
        /// <summary>
        ///     获取或设置服务可执行文件路径
        /// </summary>
        String FilePath { get; set; }
        /// <summary>
        ///     获取或设置服务可执行文件目录
        /// </summary>
        String DirectoryPath { get; set; }
    }
}