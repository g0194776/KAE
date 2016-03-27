namespace KJFramework.Hook
{
    /// <summary>
    ///        键盘钩子元接口，提供了相关的基本操作。
    /// </summary>
    public interface IKeyboardHook : IIOHook
    {
        /// <summary>
        ///        防止2次触发事件的时间间隔，默认为500毫秒
        ///            * 单位：毫秒
        /// </summary>
        int ResetMilliseconds { get; set; }
        /// <summary>
        ///     安装钩子
        /// </summary>
        /// <returns>
        ///     安装状态
        /// </returns>
        bool InstallHook();
        /// <summary>
        ///      卸载钩子
        /// </summary>
        void UnInstallHook();
    }
}
