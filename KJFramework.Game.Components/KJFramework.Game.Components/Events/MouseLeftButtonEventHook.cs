using KJFramework.EventArgs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace KJFramework.Game.Components.Events
{
    /// <summary>
    ///     鼠标左键事件钩子
    /// </summary>
    public class MouseLeftButtonEventHook : EventHook
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
            //左键点击
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (!Control.Getfocus)
                {
                    Control.Getfocus = true;
                    Control.GetFocusedHandler(new System.EventArgs());
                }
                Control.IsLeftMouseButtonPressed = true;
                Control.LeftClickedHandler(new System.EventArgs());
            }
            //左键松开
            if (mouseState.LeftButton == ButtonState.Released)
            {
                if (Control.IsLeftMouseButtonPressed)
                {
                    Control.PreMouseDragPosition = Vector2.Zero;
                    Control.IsLeftMouseButtonPressed = false;
                    Control.LeftButtonReleaseHandler(new System.EventArgs());
                }
            }
            //Drag it.
            if(Control.IsLeftMouseButtonPressed)
            {
                var vector2 = new Vector2(mouseState.X, mouseState.Y);
                if (Control.PreMouseDragPosition != Vector2.Zero && Control.PreMouseDragPosition != vector2)
                    Control.MouseDragHandler(
                        new LightSingleArgEventArgs<Vector2>(
                            new Vector2(vector2.X - Control.PreMouseDragPosition.X,
                                               vector2.Y - Control.PreMouseDragPosition.Y)));
                Control.PreMouseDragPosition = vector2;
            }
            return true;
        }

        #endregion
    }
}