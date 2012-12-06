using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace KJFramework.Game.Components.Events
{
    /// <summary>
    ///     ����Ҽ��¼�����
    /// </summary>
    public class MouseRightButtonEventHook : EventHook
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
            //�Ҽ����
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
            //�Ҽ��ɿ�
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