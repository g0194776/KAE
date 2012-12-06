using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using KJFramework.Game.Components.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KJFramework.Game.Components.Controls
{
    /// <summary>
    ///     窗体的基础表现形式，提供了相关的基本操作。
    /// </summary>
    public class Window : Control
    {
        #region 构造函数

        /// <summary>
        ///     窗体的基础表现形式，提供了相关的基本操作。
        /// </summary>
        public Window(Microsoft.Xna.Framework.Game game)
            : base(game)
        {
            MouseDrag += WindowMouseDrag;
        }

        #endregion

        #region 父类方法

        /// <summary>
        /// Called when the DrawableGameComponent needs to be drawn.  Override this method with component-specific drawing code. Reference page contains links to related conceptual articles.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Microsoft.Xna.Framework.DrawableGameComponent.Draw(Microsoft.Xna.Framework.GameTime).</param>
        public override void Draw(GameTime gameTime)
        {
            //绘制背景
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

        #region 成员

        private Texture2D _background;
        private bool _canMove;
        private ConcurrentDictionary<String, WindowControlPair> _controls = new ConcurrentDictionary<string, WindowControlPair>();

        /// <summary>
        ///     获取或设置背景
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
        ///     获取或设置一个值，该值标示了当前窗体是否能够被移动
        /// </summary>
        public bool CanMove
        {
            get { return _canMove; }
            set { _canMove = value; }
        }

        private bool _useBackgroundSetBound;
        /// <summary>
        ///     获取或设置一个值，该值标示为是否使用窗体背景图片的边界当做窗体的大小
        /// </summary>
        public bool UseBackgroundSetBound
        {
            get { return _useBackgroundSetBound; }
            set { _useBackgroundSetBound = value; }
        }


        #endregion

        #region 方法

        /// <summary>
        ///     注册一个控件
        /// </summary>
        /// <param name="name">控件名称</param>
        /// <param name="pair">控件</param>
        public void RegistControl(String name, WindowControlPair pair)
        {
            if (String.IsNullOrEmpty(name) || pair == null)
            {
                throw new System.Exception("要添加的控件拥有非法的属性值 !");
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
                //重置窗口内所有控件的位置，因为随着窗口被移动，窗口内的控件位置也会变化。
                pair.Control.Position = new Vector2(_position.X + pair.Position.X, _position.Y + pair.Position.Y);
                pair.Control.Update(new GameTime());
            }
        }

        #endregion
    }
}