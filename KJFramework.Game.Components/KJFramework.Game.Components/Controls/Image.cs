using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KJFramework.Game.Components.Controls
{
    /// <summary>
    ///     ͼƬ�ؼ������ھ�̬����ʾ��ָ�������С�
    /// </summary>
    public class Image : Control
    {
        #region ���캯��

        /// <summary>
        ///     ͼƬ�ؼ������ھ�̬����ʾ��ָ�������С�
        /// </summary>
        public Image(Microsoft.Xna.Framework.Game game, Rectangle rectangle)
            : base(game)
        {
            _width = rectangle.Width;
            _height = rectangle.Height;
            _area = rectangle;
            _position = new Vector2(rectangle.X, rectangle.Y);
        }

        #endregion

        #region ���෽��

        /// <summary>
        /// Called when the DrawableGameComponent needs to be drawn.  Override this method with component-specific drawing code. Reference page contains links to related conceptual articles.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Microsoft.Xna.Framework.DrawableGameComponent.Draw(Microsoft.Xna.Framework.GameTime).</param>
        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied);
            _spriteBatch.Draw(_source, _area, _color);
            _spriteBatch.End();
        }

        #endregion

        #region ��Ա

        #region ��������

        /// <summary>
        ///     ��ȡ��������ʾ������
        /// </summary>
        public Texture2D Source
        {
            get { return _source; }
            set { _source = value; }
        }

        /// <summary>
        ///     ��ȡ������͸����
        /// </summary>
        public float Alpha
        {
            get { return _alpha; }
            set
            {
                _alpha = value;
                _color = new Color(1F, 1F, 1F, _alpha);
            }
        }

        #endregion

        #region ˽������

        protected Texture2D _source;
        protected float _alpha;
        protected Color _color = new Color(1F, 1F, 1F, 1F);

        #endregion

        #endregion
    }
}