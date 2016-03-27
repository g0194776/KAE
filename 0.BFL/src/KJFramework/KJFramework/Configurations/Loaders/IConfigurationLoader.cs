using System;
using KJFramework.Enums;
using KJFramework.Statistics;

namespace KJFramework.Configurations.Loaders
{
    /// <summary>
    ///     配置加载器元接口，提供了相关的基本操作。
    /// </summary>
    public interface IConfigurationLoader : IStatisticable<Statistic>, IDisposable
    {
        /// <summary>
        ///     获取配置加载器类型
        /// </summary>
        ConfigurationLoaderTypes ConfigurationLoaderType { get; }
        /// <summary>
        ///     加载配置
        /// </summary>
        /// <typeparam name="T">自定义配置节类型</typeparam>
        /// <param name="action">赋值自定义配置节的动作</param>
        void Load<T>(Action<T> action) where T : class, new();
    }
}