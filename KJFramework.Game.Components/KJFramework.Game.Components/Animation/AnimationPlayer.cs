using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KJFramework.Game.Components.Animation
{
    /// <summary>
    ///     ���������������ڿ��ƶ�����ѭ�����š�
    /// </summary>
    public class  AnimationPlayer : IAnimationPlayer
    {
        #region ˽�г�Ա

        private Animation _animation;
        private int _frameIndex;
        /// <summary>
        /// The amount of _time in seconds that the current frame has been shown for.
        /// </summary>
        private float _time;

        #endregion

        #region ������Ա

        /// <summary>
        ///     ��ȡ��ǰҪ���ŵĶ���
        /// </summary>
        public Animation Animation
        {
            get { return _animation; }
        }

        /// <summary>
        ///     ��ȡ��ǰҪ���Ŷ�����֡��
        /// </summary>
        public int FrameIndex
        {
            get { return _frameIndex; }
        }

        /// <summary>
        ///     ��ȡ��ǰ����֡��ƫ��
        /// </summary>
        public Vector2 Origin
        {
            get { return new Vector2(Animation.FrameWidth / 2.0f, Animation.FrameHeight); }
        }

        #endregion

        #region IAnimationPlayer Members

        /// <summary>
        ///     ���Ŷ���
        /// </summary>
        public void PlayAnimation(Animation animation)
        {
            //�����ǰҪ���ŵĶ���û��׼���ã��򲻻Ὺʼ����
            if (Animation == animation)
                return;

            //��ʼ�µĶ���
            _animation = animation;
            _frameIndex = 0;
            _time = 0.0f;
        }

        /// <summary>
        ///     ����ʱ�������Ƶ�ǰ֡����
        /// </summary>
        /// <param name="gameTime">��Ϸʱ��</param>
        /// <param name="spriteBatch">������</param>
        /// <param name="position">λ��</param>
        /// <param name="spriteEffects">����Ч��</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects)
        {
            if (Animation == null)
                throw new NotSupportedException("��ǰû��Ҫ���ŵĶ�����");

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