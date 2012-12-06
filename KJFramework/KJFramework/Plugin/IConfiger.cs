using System;

namespace KJFramework.Plugin
{
    /// <summary>
    ///     配置器元接口, 提供了相关的基本操作。
    /// </summary>
    public interface IConfiger
    {
        /// <summary>
        ///     获取配置文件路径
        /// </summary>
        String ConfigFile { get; }
        /// <summary>
        ///     保存配置
        /// </summary>
        /// <returns>
        ///     返回false, 表示保存失败。
        /// </returns>
        bool Save();
    }
}
