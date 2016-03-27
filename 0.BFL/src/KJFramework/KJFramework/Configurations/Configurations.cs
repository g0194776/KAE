using System;
using KJFramework.Configurations.Loaders;

namespace KJFramework.Configurations
{
    /// <summary>
    ///     配置节管理器，提供了相关的基本操作
    /// </summary>
    public static class Configurations
    {
        #region Members

        /// <summary>
        ///     远程配置加载器
        ///     <para>* 如果此字段值为空，则会使用本地配置文件加载器</para>
        /// </summary>
        public static IConfigurationLoader RemoteConfigLoader { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     获取自定义配置节
        ///            * 当前默认加载方式为：本地加载
        /// </summary>
        /// <typeparam name="T">自定义配置节类型</typeparam>
        /// <param name="action">赋值自定义配置节的动作</param>
        public static void GetConfiguration<T>(Action<T> action) where T : class, new()
        {
            if (RemoteConfigLoader == null)
                using (LocalConfigurationLoader loader = new LocalConfigurationLoader()) loader.Load(action);
            else RemoteConfigLoader.Load(action);
        }

        #endregion
    }
}