using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KJFramework.Game.Components.Controls
{
    /// <summary>
    ///     图片控件，用于静态的显示在指定容器中。
    /// </summary>
    public class Image : Control
    {
        #region 构造函数

        /// <summary>
        ///     图片控件，用于静态的显示在指定容器中。
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

        #region 父类方法

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

        #region 成员

        #region 公有属性

        /// <summary>
        ///     获取或设置显示的内容
        /// </summary>
        public Texture2D Source
        {
            get { return _source; }
            set { _source = value; }
        }

        /// <summary>
        ///     获取或设置透明度
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

        #region 私有属性

        protected Texture2D _source;
        protected float _alpha;
        protected Color _color = new Color(1F, 1F, 1F, 1F);

        #endregion

        #endregion
    }
}