using KJFramework.Basic.Enum;

namespace KJFramework.Plugin
{
    /// <summary>
    ///     插件元接口, 提供了相关的基本操作
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        ///     获取或设置插件信息
        /// </summary>
        PluginInfomation PluginInfo { get; }
        /// <summary>
        ///      获取或设置可用标示
        /// </summary>
        bool Enable { get; set; }
        /// <summary>
        ///     获取或设置插件类型
        /// </summary>
        PluginTypes PluginType { get; }
        /// <summary>
        ///     加载后需要做的动作
        /// </summary>
        void OnLoading();
    }
}
