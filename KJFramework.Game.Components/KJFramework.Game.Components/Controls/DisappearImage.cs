using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KJFramework.Game.Components.Controls
{
    /// <summary>
    ///     ����ʧ��ͼƬЧ������Ҫ����LOGO�Ŀ�ͷ��ʾ
    /// </summary>
    public class DisappearImage : Image
    {
        #region ���캯��

        /// <summary>
        ///     ����ʧ��ͼƬЧ������Ҫ����LOGO�Ŀ�ͷ��ʾ
        /// </summary>
        public DisappearImage(Microsoft.Xna.Framework.Game game, Rectangle rectangle)
            : base(game, rectangle)
        {
            Alpha = 0F;
        }

        #endregion

        #region ��Ա

        #region ˽������

        private float _time;
        private float _frameTime;
        private bool _isGrouping = true;
        private DateTime _topTime;
        private int _waitSecond = 3;

        #endregion

        #region ��������

        /// <summary>
        ///     ��ȡ������ÿһ֡��ʾ��ʱ��
        /// </summary>
        public float FrameTime
        {
            get { return _frameTime; }
            set { _frameTime = value; }
        }

        /// <summary>
        ///     ��ȡ�����õȴ�ʱ��(��λ����)
        ///     <para>* Ĭ��ֵΪ��3��</para>
        /// </summary>
        public int WaitSecond
        {
            get { return _waitSecond; }
            set { _waitSecond = value; }
        }

        #endregion

        #endregion

        #region ���෽��

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
                //͸����UP
                if (_isGrouping && Alpha < 1F) Alpha += _frameTime;
                //ת��״̬����ʼ��ʧ
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