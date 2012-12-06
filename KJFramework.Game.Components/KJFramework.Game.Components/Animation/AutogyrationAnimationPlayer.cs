using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KJFramework.Game.Components.Animation
{
    /// <summary>
    ///   ����ת����������������ṩ����صĻ���������
    /// </summary>
    public class AutogyrationAnimationPlayer : DrawableGameComponent, IAnimationPlayer
    {
        #region ��Ա

        private float _addAngle;
        private float _angle;
        private SpriteBatch _spriteBatch;
        /// <summary>
        /// The amount of _time in seconds that the current frame has been shown for.
        /// </summary>
        private float _time;

        #endregion

        #region ���캯��

        /// <summary>
        ///   ����ת����������������ṩ����صĻ���������
        /// </summary>
        /// <param name="game">��Ϸ����</param>
        public AutogyrationAnimationPlayer(Microsoft.Xna.Framework.Game game)
            : base(game)
        {
            _spriteBatch = new SpriteBatch(game.GraphicsDevice);
        }

        #endregion

        #region IAnimationPlayer ��Ա

        private Animation _animation;

        /// <summary>
        ///     ��ȡ��ǰҪ���ŵĶ���
        /// </summary>
        public Animation Animation
        {
            get { return _animation; }
        }

        private int _frameIndex;
        /// <summary>
        ///     ��ȡ��������תһ�ܵĴ���
        /// </summary>
        [Obsolete("������ת�Ķ����������У����ֶ��Ѿ�����ʹ�á�", true)]
        public int FrameIndex
        {
            get { return _frameIndex; }
            set { _frameIndex = value; }
        }

        private Vector2 _origin;
        /// <summary>
        ///    ��ȡ�����ö�����λ��
        /// </summary>
        public Vector2 Origin
        {
            get { return _origin; }
            set { _origin = value;}
        }

        /// <summary>
        ///     ���Ŷ���
        /// </summary>
        public void PlayAnimation(Animation animation)
        {
            if (_animation == null)
            {
                _animation = animation;
                _angle = 360F /animation.FrameCount;
            }
        }

        #endregion

        #region ���෽��

        /// <summary>
        /// Called when the DrawableGameComponent needs to be drawn.  Override this method with component-specific drawing code. Reference page contains links to related conceptual articles.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Microsoft.Xna.Framework.DrawableGameComponent.Draw(Microsoft.Xna.Framework.GameTime).</param>
        public override void Draw(GameTime gameTime)
        {
            if (_addAngle >= 0)
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(_animation.Texture,
                                  new Rectangle((int)_origin.X, (int)_origin.Y, _animation.Texture.Width,
                                                _animation.Texture.Height),
                                  null, Color.White, _addAngle,
                                  new Vector2(_animation.Texture.Width / 2, _animation.Texture.Height / 2),
                                  SpriteEffects.None, 0);
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
            _time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            while (_time > Animation.FrameTime)
            {
                _time -= Animation.FrameTime;
                if (_addAngle >= 360F)
                {
                    if (_animation.IsLooping)
                    {
                        _addAngle = 0;
                    }
                    else
                    {
                        _addAngle = -1F;
                    }
                }
                if (_animation.IsLooping)
                {
                    _addAngle += _angle;
                }
            }
            base.Update(gameTime);
        }

        #endregion
    }
}