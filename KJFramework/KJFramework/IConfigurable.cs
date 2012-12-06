using System;
using KJFramework.Plugin;

namespace KJFramework
{
    /// <summary>
    ///     可配置元接口，提供了将一个配置文件附属到一个对向上的基本操作。
    /// </summary>
    public interface IConfigurable
    {
        /// <summary>
        ///     读取配置文件
        /// </summary>
        /// <param name="path">配置文件全路径</param>
        /// <returns>返回一个配置器</returns>
        IConfiger Config(String path);
        /// <summary>
        ///     读取配置文件
        /// </summary>
        /// <param name="path">配置文件全路径</param>
        /// <param name="args">可扩展的配置参数</param>
        /// <returns>返回一个配置器</returns>
        IConfiger Config(String path, Object args);
        /// <summary>
        ///     读取配置文件
        /// </summary>
        /// <param name="path">配置文件全路径</param>
        /// <param name="args">可扩展的配置参数集合</param>
        /// <returns>返回一个配置器</returns>
        IConfiger Config(String path, Object[] args);
        /// <summary>
        ///     获取已经存在的配置器
        /// </summary>
        /// <returns>返回已经存在的配置器，如果未经过配置，则返回null。</returns>
        IConfiger GetConfiger();
    }
}