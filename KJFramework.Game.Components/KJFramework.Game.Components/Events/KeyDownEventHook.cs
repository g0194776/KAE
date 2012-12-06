using KJFramework.Game.Components.EventArgs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace KJFramework.Game.Components.Events
{
    /// <summary>
    ///     �����¼�����
    /// </summary>
    public class KeyDownEventHook : EventHook
    {
        #region Overrides of EventHook

        /// <summary>
        ///     ִ��һ���¼�����
        /// </summary>
        /// <param name="keyboardState">����״̬</param>
        /// <param name="mouseState">���״̬</param>
        /// <param name="gameTime">��Ϸʱ��</param>
        /// <returns>�����Ƿ���Ҫ������һ�����ӵı�ʾλ</returns>
        public override bool Execute(KeyboardState keyboardState, MouseState mouseState, GameTime gameTime)
        {
            Keys[] keys = keyboardState.GetPressedKeys();
            //�ȴ�ʱ���ӳ����������㰴�����
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