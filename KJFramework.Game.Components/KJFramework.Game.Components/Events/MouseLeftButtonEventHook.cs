using KJFramework.EventArgs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace KJFramework.Game.Components.Events
{
    /// <summary>
    ///     �������¼�����
    /// </summary>
    public class MouseLeftButtonEventHook : EventHook
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
            //������
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
            //����ɿ�
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