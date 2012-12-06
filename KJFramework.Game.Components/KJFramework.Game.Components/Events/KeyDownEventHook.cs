using KJFramework.Game.Components.EventArgs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace KJFramework.Game.Components.Events
{
    /// <summary>
    ///     按键事件钩子
    /// </summary>
    public class KeyDownEventHook : EventHook
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
            Keys[] keys = keyboardState.GetPressedKeys();
            //等待时间延长，用来计算按键间隔
            Control.WaitTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (keys != null && keys.Length > 0)
            {
                if (Control.WaitTime - Control.PreTime >= 0.1F || Control.PreKeys != keys[0])
                {
                    Control.PreKeys = keys[0];
                    Control.PreTime = Control.WaitTime;
                    Control.WaitTime -= 0.1F;
                    if (keyboardState.IsKeyDown(Keys.CapsLock))
                    {
                        Control.IsCapsLock = !Control.IsCapsLock;
                    }
                    Control.KeyDownHandler(new KeyDownEventArgs(keys[0], Control.IsCapsLock));
                }
            }
            return true;
        }

        #endregion
    }
}