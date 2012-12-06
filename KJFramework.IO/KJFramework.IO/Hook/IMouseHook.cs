using System;
using KJFramework.IO.EventArgs;

namespace KJFramework.IO.Hook
{
    /// <summary>
    ///     鼠标钩子元接口，提供了相关的基本操作。
    /// </summary>
    public interface IMouseHook : IIOHook
    {
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
        /// <summary>
        ///     鼠标移动事件
        /// </summary>
        event EventHandler<MouseMoveEventArgs> MouseMove;
    }
}