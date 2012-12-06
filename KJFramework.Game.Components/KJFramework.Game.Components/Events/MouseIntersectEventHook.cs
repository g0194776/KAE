using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace KJFramework.Game.Components.Events
{
    /// <summary>
    ///     �����ײ����¼�����
    /// </summary>
    public class MouseIntersectEventHook : EventHook
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