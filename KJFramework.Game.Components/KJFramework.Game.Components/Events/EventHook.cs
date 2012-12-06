using KJFramework.Game.Components.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace KJFramework.Game.Components.Events
{
    /// <summary>
    ///     事件钩子，提供了相关的基本操作
    /// </summary>
    public abstract class EventHook : IEventHook
    {
        #region Members

        /// <summary>
        ///     获取或设置内部控件
        /// </summary>
        public Control Control { get; set; }

        #endregion

        #region Implementation of IEventHook

        /// <summary>
        ///     执行一个事件钩子
        /// </summary>
        /// <param name="keyboardState">键盘状态</param>
        /// <param name="mouseState">鼠标状态</param>
        /// <param name="gameTime">游戏时间</param>
        /// <returns>返回是否需要出发下一个钩子的标示位</returns>
        public abstract bool Execute(KeyboardState keyboardState, MouseState mouseState, GameTime gameTime);

        #endregion
    }
}