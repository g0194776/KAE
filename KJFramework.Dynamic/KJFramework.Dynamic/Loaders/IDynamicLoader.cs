using System;

namespace KJFramework.Dynamic.Loaders
{
    /// <summary>
    ///     动态加载器元接口，提供了相关的基本操作。
    /// </summary>
    public interface IDynamicLoader
    {
        /// <summary>
        ///     动态加载
        /// </summary>
        /// <param name="args">加载参数</param>
        void Load(params String[] args);
        /// <summary>
        ///     加载成功事件
        /// </summary>
        event EventHandler LoadSuccessfully;
        /// <summary>
        ///     加载失败事件
        /// </summary>
        event EventHandler LoadFailed;
    }
}