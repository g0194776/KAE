using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KJFramework.Game.Components.Controls
{
    /// <summary>
    ///     逐渐消失的图片效果，主要用于LOGO的开头显示
    /// </summary>
    public class DisappearImage : Image
    {
        #region 构造函数

        /// <summary>
        ///     逐渐消失的图片效果，主要用于LOGO的开头显示
        /// </summary>
        public DisappearImage(Microsoft.Xna.Framework.Game game, Rectangle rectangle)
            : base(game, rectangle)
        {
            Alpha = 0F;
        }

        #endregion

        #region 成员

        #region 私有属性

        private float _time;
        private float _frameTime;
        private bool _isGrouping = true;
        private DateTime _topTime;
        private int _waitSecond = 3;

        #endregion

        #region 公有属性

        /// <summary>
        ///     获取或设置每一帧显示的时间
        /// </summary>
        public float FrameTime
        {
            get { return _frameTime; }
            set { _frameTime = value; }
        }

        /// <summary>
        ///     获取或设置等待时常(单位：秒)
        ///     <para>* 默认值为：3秒</para>
        /// </summary>
        public int WaitSecond
        {
            get { return _waitSecond; }
            set { _waitSecond = value; }
        }

        #endregion

        #endregion

        #region 父类方法

        /// <summary>
        /// Called when the DrawableGameComponent needs to be drawn.  Override this method with component-specific drawing code. Reference page contains links to related conceptual articles.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Microsoft.Xna.Framework.DrawableGameComponent.Draw(Microsoft.Xna.Framework.GameTime).</param>
        public override void Draw(GameTime gameTime)
        {
            // Process passing _time.
            _time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            while (_time > _frameTime)
            {
                _time -= _frameTime;
                //透明度UP
                if (_isGrouping && Alpha < 1F) Alpha += _frameTime;
                //转换状态，开始消失
                if (_isGrouping && Alpha >= 1F)
                {
                    _topTime = DateTime.Now;
                    _isGrouping = false;
                }
                //DOWN && Wait some second.
                if (!_isGrouping && (DateTime.Now - _topTime).TotalSeconds >= 3) Alpha -= _frameTime;
                if (!_isGrouping && Alpha <= 0F)
                {
                    Game.Components.Remove(this);
                    return;
                }
            }
            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied);
            _spriteBatch.Draw(_source, _area, _color);
            _spriteBatch.End();
        }

        #endregion
    }
}