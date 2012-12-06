using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using KJFramework.Game.Components.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KJFramework.Game.Components.Controls
{
    /// <summary>
    ///     ����Ļ���������ʽ���ṩ����صĻ���������
    /// </summary>
    public class Window : Control
    {
        #region ���캯��

        /// <summary>
        ///     ����Ļ���������ʽ���ṩ����صĻ���������
        /// </summary>
        public Window(Microsoft.Xna.Framework.Game game)
            : base(game)
        {
            MouseDrag += WindowMouseDrag;
        }

        #endregion

        #region ���෽��

        /// <summary>
        /// Called when the DrawableGameComponent needs to be drawn.  Override this method with component-specific drawing code. Reference page contains links to related conceptual articles.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Microsoft.Xna.Framework.DrawableGameComponent.Draw(Microsoft.Xna.Framework.GameTime).</param>
        public override void Draw(GameTime gameTime)
        {
            //���Ʊ���
            if (_background != null)
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(_background, _position, Color.White);
                _spriteBatch.End();
            }
            foreach (WindowControlPair pair in _controls.Values)
            {
                pair.Control.Draw(gameTime);
            }
        }

        /// <summary>
        /// Called when the GameComponent needs to be updated.  Override this method with component-specific update code.
        /// </summary>
        /// <param name="gameTime">Time elapsed since the last call to Microsoft.Xna.Framework.GameComponent.Update(Microsoft.Xna.Framework.GameTime)</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            foreach (WindowControlPair pair in _controls.Values)
            {
                pair.Control.Update(gameTime);
            }
        }

        #endregion

        #region ��Ա

        private Texture2D _background;
        private bool _canMove;
        private ConcurrentDictionary<String, WindowControlPair> _controls = new ConcurrentDictionary<string, WindowControlPair>();

        /// <summary>
        ///     ��ȡ�����ñ���
        /// </summary>
        public Texture2D Background
        {
            get { return _background; }
            set
            {
                _background = value;
                if (_background != null && _useBackgroundSetBound)
                {
                    _width = _background.Width;
                    _height = _background.Height;
                }
            }
        }

        /// <summary>
        ///     ��ȡ������һ��ֵ����ֵ��ʾ�˵�ǰ�����Ƿ��ܹ����ƶ�
        /// </summary>
        public bool CanMove
        {
            get { return _canMove; }
            set { _canMove = value; }
        }

        private bool _useBackgroundSetBound;
        /// <summary>
        ///     ��ȡ������һ��ֵ����ֵ��ʾΪ�Ƿ�ʹ�ô��屳��ͼƬ�ı߽統������Ĵ�С
        /// </summary>
        public bool UseBackgroundSetBound
        {
            get { return _useBackgroundSetBound; }
            set { _useBackgroundSetBound = value; }
        }


        #endregion

        #region ����

        /// <summary>
        ///     ע��һ���ؼ�
        /// </summary>
        /// <param name="name">�ؼ�����</param>
        /// <param name="pair">�ؼ�</param>
        public void RegistControl(String name, WindowControlPair pair)
        {
            if (String.IsNullOrEmpty(name) || pair == null)
            {
                throw new System.Exception("Ҫ��ӵĿؼ�ӵ�зǷ�������ֵ !");
            }
            pair.Control.Position = new Vector2(_position.X + pair.Position.X, _position.Y + pair.Position.Y);
            _controls.TryAdd(name, pair);
        }

        #endregion

        #region Events

        void WindowMouseDrag(object sender, KJFramework.EventArgs.LightSingleArgEventArgs<Vector2> e)
        {
            Debug.Print(e.Target.X + ", " + e.Target.Y + " ||| " + Position.X + ", " + Position.Y);
            if (!_canMove) return;
            _position.X +=  e.Target.X;
            _position.Y += e.Target.Y;
            foreach (WindowControlPair pair in _controls.Values)
            {
                //���ô��������пؼ���λ�ã���Ϊ���Ŵ��ڱ��ƶ��������ڵĿؼ�λ��Ҳ��仯��
                pair.Control.Position = new Vector2(_position.X + pair.Position.X, _position.Y + pair.Position.Y);
                pair.Control.Update(new GameTime());
            }
        }

        #endregion
    }
}