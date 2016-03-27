using System;
using KJFramework.Services;

namespace KJFramework.Installers
{
    /// <summary>
    ///     服务安装器元接口，提供了相关的基本操作。
    /// </summary>
    /// <typeparam name="T">安装对象</typeparam>
    public interface IServiceInstaller<T> : IDisposable
    {
        /// <summary>
        ///     安装服务
        /// </summary>
        /// <param name="obj">安装对象</param>
        /// <returns>返回安装的状态</returns>
        IWindowsService Install(T obj);
        /// <summary>
        ///     卸载服务
        /// </summary>
        /// <param name="name">服务名</param>
        /// <returns>返回写在状态</returns>
        bool UnInstall(String name);
    }
}