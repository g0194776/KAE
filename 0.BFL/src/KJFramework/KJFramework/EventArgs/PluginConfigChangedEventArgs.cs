using System;

namespace KJFramework.EventArgs
{
    public delegate void DELEGATE_PLUGINCONFIG_CHANGED(Object sender, PluginConfigChangedEventArgs e);
    /// <summary>
    ///     插件配置文件更改事件
    /// </summary>
    public class PluginConfigChangedEventArgs : System.EventArgs
    {
        /// <summary>
        ///     插件配置文件更改事件
        /// </summary>
        public PluginConfigChangedEventArgs()
        {
        }
    }
}
