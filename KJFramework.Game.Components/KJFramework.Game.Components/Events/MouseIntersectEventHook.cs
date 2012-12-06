using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace KJFramework.Game.Components.Events
{
    /// <summary>
    ///     鼠标碰撞检测事件钩子
    /// </summary>
    public class MouseIntersectEventHook : EventHook
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
            Control.CurrentMouseState = mouseState;
            Rectangle rectangle = new Rectangle(mouseState.X, mouseState.Y, 1, 1);
            //check range.
            if(Control.Area.Intersects(rectangle))
            {
                //move enter.
                Control.IsMouseEnter = true;
                Control.MouseEnterHandler(new System.EventArgs());
                return true;
            }
            if (Control.IsMouseEnter)
            {
                Control.IsMouseEnter = false;
                Control.MouseLeaveHandler(new System.EventArgs());
            }
            return Control.IsLeftMouseButtonPressed;
        }

        #endregion
    }
}