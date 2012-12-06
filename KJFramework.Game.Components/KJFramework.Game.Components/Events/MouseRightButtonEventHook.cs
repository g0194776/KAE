using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace KJFramework.Game.Components.Events
{
    /// <summary>
    ///     鼠标右键事件钩子
    /// </summary>
    public class MouseRightButtonEventHook : EventHook
    {
        #region Overrides of EventHook

        /// <summary>
        ///     执行一个事件钩子
        /// </summary>
        /// <param name="keyboardState">键盘状态</param>
        /// <param name="mouseState">鼠标状态</param>
        /// <param name="gameTime">游戏时间</param>
        /// <returns>返回是否需要出发下一个钩子的标示位</returns>
        public override bool Execute(KeyboardState keyboardState, MouseState mouseState, GameTime gameTime)
        {
            //右键点击
            if (mouseState.RightButton == ButtonState.Pressed)
            {
                if (!Control.Getfocus)
                {
                    Control.Getfocus = true;
                    Control.GetFocusedHandler(new System.EventArgs());
                }
                Control.IsRightMouseButtonPressed = true;
                Control.RightClickedHandler(new System.EventArgs());
            }
            //右键松开
            if (mouseState.RightButton == ButtonState.Released)
            {
                if (Control.IsRightMouseButtonPressed)
                {
                    Control.IsRightMouseButtonPressed = false;
                    Control.RightButtonReleaseHandler(new System.EventArgs());
                }
            }
            return true;
        }

        #endregion
    }
}