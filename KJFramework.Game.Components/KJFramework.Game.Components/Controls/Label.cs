using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KJFramework.Game.Components.Controls
{
    /// <summary>
    ///     标签，用于在界面上显示固定的文字
    /// </summary>
    public class Label : Control
    {
        #region 构造函数

        /// <summary>
        ///     标签，用于在界面上显示固定的文字
        /// </summary>
        public Label(Microsoft.Xna.Framework.Game game)
            : base(game)
        {
            
        }

        /// <summary>
        ///     标签，用于在界面上显示固定的文字
        /// </summary>
        public Label(Microsoft.Xna.Framework.Game game, Vector2 position)
            : base(game)
        {
            _position = position;
        }

        #endregion

        #region 父类方法

        /// <summary>
        /// Called when the DrawableGameComponent needs to be drawn.  Override this method with component-specific drawing code. Reference page contains links to related conceptual articles.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Microsoft.Xna.Framework.DrawableGameComponent.Draw(Microsoft.Xna.Framework.GameTime).</param>
        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            if (_background != null)
            {
                _spriteBatch.Draw(_background, new Rectangle((int)_position.X, (int)_position.Y, _backgroundWidth, _backgroundHeight), Color.White);
            }
            _spriteBatch.DrawString(_font, _caption, _position, _fontColor);
            _spriteBatch.End();
        }

        #endregion

        #region 公有属性

        private String _caption;
        /// <summary>
        ///     获取或设置显示的内容
        /// </summary>
        public String Caption
        {
            get { return _caption; }
            set { _caption = value; }
        }

        private SpriteFont _font;
        /// <summary>
        ///     获取或设置显示的文字字体
        /// </summary>
        public SpriteFont Font
        {
            get { return _font; }
            set { _font = value; }
        }

        private Color _fontColor = Color.Black;
        /// <summary>
        ///     获取或设置文字颜色
        /// </summary>
        public Color FontColor
        {
            get { return _fontColor; }
            set { _fontColor = value; }
        }

        private Texture2D _background;
        /// <summary>
        ///     获取或设置文字的背景
        /// </summary>
        public Texture2D Background
        {
            get { return _background; }
            set { _background = value; }
        }

        /// <summary>
        ///     获取或设置背景宽
        /// </summary>
        public int BackgroundWidth
        {
            get { return _backgroundWidth; }
            set { _backgroundWidth = value; }
        }

        /// <summary>
        ///     获取或设置背景高
        /// </summary>
        public int BackgroundHeight
        {
            get { return _backgroundHeight; }
            set { _backgroundHeight = value; }
        }

        private int _backgroundWidth;
        private int _backgroundHeight;


        #endregion
    }
}