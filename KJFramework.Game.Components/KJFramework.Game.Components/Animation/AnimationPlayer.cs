using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KJFramework.Game.Components.Animation
{
    /// <summary>
    ///     动画播放器，用于控制动画的循环播放。
    /// </summary>
    public class  AnimationPlayer : IAnimationPlayer
    {
        #region 私有成员

        private Animation _animation;
        private int _frameIndex;
        /// <summary>
        /// The amount of _time in seconds that the current frame has been shown for.
        /// </summary>
        private float _time;

        #endregion

        #region 公共成员

        /// <summary>
        ///     获取当前要播放的动画
        /// </summary>
        public Animation Animation
        {
            get { return _animation; }
        }

        /// <summary>
        ///     获取当前要播放动画的帧数
        /// </summary>
        public int FrameIndex
        {
            get { return _frameIndex; }
        }

        /// <summary>
        ///     获取当前动画帧的偏移
        /// </summary>
        public Vector2 Origin
        {
            get { return new Vector2(Animation.FrameWidth / 2.0f, Animation.FrameHeight); }
        }

        #endregion

        #region IAnimationPlayer Members

        /// <summary>
        ///     播放动画
        /// </summary>
        public void PlayAnimation(Animation animation)
        {
            //如果当前要播放的动画没有准备好，则不会开始播放
            if (Animation == animation)
                return;

            //开始新的动画
            _animation = animation;
            _frameIndex = 0;
            _time = 0.0f;
        }

        /// <summary>
        ///     根据时间来绘制当前帧动画
        /// </summary>
        /// <param name="gameTime">游戏时间</param>
        /// <param name="spriteBatch">精灵组</param>
        /// <param name="position">位置</param>
        /// <param name="spriteEffects">精灵效果</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects)
        {
            if (Animation == null)
                throw new NotSupportedException("当前没有要播放的动画。");

            // Process passing _time.
            _time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            while (_time > Animation.FrameTime)
            {
                _time -= Animation.FrameTime;

                // Advance the frame index; looping or clamping as appropriate.
                if (Animation.IsLooping)
                {
                    _frameIndex = (_frameIndex + 1) % Animation.FrameCount;
                }
                else
                {
                    _frameIndex = Math.Min(_frameIndex + 1, Animation.FrameCount - 1);
                }
            }

            // Calculate the source rectangle of the current frame.
            Rectangle source = new Rectangle(FrameIndex * Animation.Texture.Height, 0, Animation.Texture.Height, Animation.Texture.Height);

            // Draw the current frame.
            spriteBatch.Draw(Animation.Texture, position, source, Color.White, 0.0f, Origin, 1.0f, spriteEffects, 0.0f);
        }

        #endregion
    }
}