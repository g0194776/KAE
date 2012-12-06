using System;
using KJFramework.Attribute;
using KJFramework.Configurations.Loaders;
using KJFramework.Helpers;

namespace KJFramework.Configurations
{
    /// <summary>
    ///     配置节管理器，提供了相关的基本操作
    /// </summary>
    public class Configurations
    {
        #region 方法

        /// <summary>
        ///     获取自定义配置节
        ///            * 当前默认加载方式为：本地加载
        /// </summary>
        /// <typeparam name="T">自定义配置节类型</typeparam>
        /// <param name="action">赋值自定义配置节的动作</param>
        public static void GetConfiguration<T>(Action<T> action) where T : class, new()
        {
            using (LocalConfigurationLoader loader = new LocalConfigurationLoader())
            {
                loader.Load(action);
            }
        }

        #endregion
    }
}