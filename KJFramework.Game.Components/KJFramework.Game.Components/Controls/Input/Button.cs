using System.Collections.Generic;
using KJFramework.Game.Basic.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace KJFramework.Game.Components.Controls.Input
{
    /// <summary>
    ///     按钮类，提供了相关的基本操作。
    /// </summary>
    public class Button : Control
    {
        #region 构造函数

        /// <summary>
        ///     按钮类，提供了相关的基本操作。
        /// </summary>
        public Button(Microsoft.Xna.Framework.Game game)
            : base(game)
        {
            InitializeResource();
        }

        #endregion

        #region 成员

        private Texture2D _currentImage;

        private Dictionary<ButtonStateTypes, Song> _sounds;
        private Dictionary<ButtonStateTypes, Texture2D> _images;
        /// <summary>
        ///     获取或设置按钮状态图片集合
        /// </summary>
        public Dictionary<ButtonStateTypes, Texture2D> Images
        {
            get { return _images; }
            set { _images = value; }
        }
        /// <summary>
        ///     获取或设置按钮胡僧因集合
        /// </summary>
        public Dictionary<ButtonStateTypes, Song> Sounds
        {
            get { return _sounds; }
            set { _sounds = value; }
        }

        private int _id;
        /// <summary>
        ///     获取或设置按钮唯一编号
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        #endregion

        #region 方法

        /// <summary>
        ///     初始化
        /// </summary>
        protected void InitializeResource()
        {
            _images = new Dictionary<ButtonStateTypes, Texture2D>(); 
            _sounds = new Dictionary<ButtonStateTypes, Song>();
        }

        /// <summary>
        ///     播放按钮声音
        /// </summary>
        /// <param name="buttonStateTypes">按钮状态</param>
        private void PlaySound(ButtonStateTypes buttonStateTypes)
        {
            Song song = null;
            if (_sounds.ContainsKey(buttonStateTypes))
            {
                song = _sounds[buttonStateTypes];
            }
            if (song != null)
            {
                MediaPlayer.Play(song);
            }
        }

        #endregion

        #region 父类方法

        /// <summary>
        /// Called when the DrawableGameComponent needs to be drawn.  Override this method with component-specific drawing code. Reference page contains links to related conceptual articles.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Microsoft.Xna.Framework.DrawableGameComponent.Draw(Microsoft.Xna.Framework.GameTime).</param>
        public override void Draw(GameTime gameTime)
        {
            if (Enabled)
            {
                //检测顺序 ： -> 先检测是否点击，因为有可能鼠标在空间区域内，但是也点击了。
                if (IsLeftMouseButtonPressed || IsRightMouseButtonPressed)
                {
                    _currentImage = _images.ContainsKey(ButtonStateTypes.Click) ? _images[ButtonStateTypes.Click] : null;
                }
                else if (IsMouseEnter)
                {
                    _currentImage = _images.ContainsKey(ButtonStateTypes.MouseEnter) ? _images[ButtonStateTypes.MouseEnter] : null;
                }
                else
                {
                    _currentImage = _images.ContainsKey(ButtonStateTypes.Normal) ? _images[ButtonStateTypes.Normal] : null;
                }
            }
            else
            {
                _currentImage = _images.ContainsKey(ButtonStateTypes.UnEnable) ? _images[ButtonStateTypes.UnEnable] : null;
            }
            //如果要绘制的纹理存在
            if (_currentImage != null)
            {
                SpriteBatch.Begin();
                SpriteBatch.Draw(_currentImage, _position, Color.White);
                SpriteBatch.End();
            }
        }

        /// <summary>
        /// Called when the GameComponent needs to be updated.  Override this method with component-specific update code.
        /// </summary>
        /// <param name="gameTime">Time elapsed since the last call to Microsoft.Xna.Framework.GameComponent.Update(Microsoft.Xna.Framework.GameTime)</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            ButtonStateTypes buttonStateTypes;
            if (Enabled)
            {
                //检测顺序 ： -> 先检测是否点击，因为有可能鼠标在空间区域内，但是也点击了。
                if (IsLeftMouseButtonPressed || IsRightMouseButtonPressed)
                {
                    buttonStateTypes = ButtonStateTypes.Click;
                }
                else if (IsMouseEnter)
                {
                    buttonStateTypes = ButtonStateTypes.MouseEnter;
                }
                else
                {
                    buttonStateTypes = ButtonStateTypes.Normal;
                }
            }
            else
            {
                buttonStateTypes = ButtonStateTypes.UnEnable;
            }
            //播放声音
            if (buttonStateTypes != ButtonStateTypes.UnEnable)
            {
                PlaySound(buttonStateTypes);
            }
        }

        #endregion
    }
}