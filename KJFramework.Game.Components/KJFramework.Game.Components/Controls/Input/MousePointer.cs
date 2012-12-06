using System.Collections.Generic;
using KJFramework.Game.Basic.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace KJFramework.Game.Components.Controls.Input
{
    /// <summary>
    ///     ���ָ�����
    /// </summary>
    public class MousePointer : DrawableGameComponent
    {
        #region ��Ա

        private bool _isLeftButtonDown;
        private bool _isRightButtonDown;
        private bool _isMiddleButtonDown;
        private SpriteBatch _spriteBatch;
        private Texture2D _currentImage;
        private Dictionary<MouseStateTypes, Texture2D> _images = new Dictionary<MouseStateTypes, Texture2D>();
        /// <summary>
        ///     ��ȡ���������ָ��״̬ͼƬ����
        /// </summary>
        public Dictionary<MouseStateTypes, Texture2D> Images
        {
            get { return _images; }
        }
        
        #endregion

        #region ���캯��

        /// <summary>
        ///     ���ָ�����
        /// </summary>
        /// <param name="game">��Ϸ����</param>
        public MousePointer(Microsoft.Xna.Framework.Game game)
            : base(game)
        {
            _spriteBatch = new SpriteBatch(game.GraphicsDevice);
        }

        #endregion

        #region ����

        /// <summary>
        ///     �������ָ��ͼƬ
        /// </summary>
        /// <param name="mouseStateTypes"></param>
        private void SetCurrentImage(MouseStateTypes mouseStateTypes)
        {
            Texture2D img;
            if (_images.TryGetValue(mouseStateTypes, out img)) _currentImage = img;
        }

        #endregion

        #region ���෽��

        /// <summary>
        /// Called when the DrawableGameComponent needs to be drawn.  Override this method with component-specific drawing code. Reference page contains links to related conceptual articles.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Microsoft.Xna.Framework.DrawableGameComponent.Draw(Microsoft.Xna.Framework.GameTime).</param>
        public override void Draw(GameTime gameTime)
        {
            MouseState state = Mouse.GetState();
            if (_currentImage != null)
            {
               _spriteBatch.Begin();
                _spriteBatch.Draw(_currentImage, new Vector2(state.X, state.Y), Color.White);
                _spriteBatch.End();
            }
            base.Draw(gameTime);
        }

        /// <summary>
        /// Called when the GameComponent needs to be updated.  Override this method with component-specific update code.
        /// </summary>
        /// <param name="gameTime">Time elapsed since the last call to Microsoft.Xna.Framework.GameComponent.Update(Microsoft.Xna.Framework.GameTime)</param>
        public override void Update(GameTime gameTime)
        {
            MouseState state = Mouse.GetState();
            if (state.LeftButton == ButtonState.Pressed)
            {
                _isLeftButtonDown = true;
                SetCurrentImage(MouseStateTypes.LeftButtonDown);
            }
            else if (state.LeftButton == ButtonState.Released && _isLeftButtonDown)
            {
                _isLeftButtonDown = false;
                SetCurrentImage(MouseStateTypes.LeftButtonRelease);
            }
            else if (state.RightButton == ButtonState.Pressed)
            {
                _isRightButtonDown = true;
                SetCurrentImage(MouseStateTypes.RightButtonDown);
            }
            else if (state.RightButton == ButtonState.Released && _isRightButtonDown)
            {
                _isRightButtonDown = false;
                SetCurrentImage(MouseStateTypes.RightButtonRelease);
            }
            else if (state.MiddleButton == ButtonState.Pressed)
            {
                _isMiddleButtonDown = true;
                SetCurrentImage(MouseStateTypes.MiddleButtonDown);
            }
            else if (state.MiddleButton == ButtonState.Released && _isMiddleButtonDown)
            {
                _isMiddleButtonDown = false;
                SetCurrentImage(MouseStateTypes.MiddleButtonRelease);
            }
            //����ƶ�
            if (!_isLeftButtonDown && !_isRightButtonDown && !_isMiddleButtonDown)
            {
                SetCurrentImage(MouseStateTypes.Normal);
            }
            base.Update(gameTime);
        }

        #endregion
    }
}