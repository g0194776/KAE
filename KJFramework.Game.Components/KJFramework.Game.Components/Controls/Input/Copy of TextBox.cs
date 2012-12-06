using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KJFramework.Game.Components.Controls.Input
{
    /// <summary>
    ///     文本框父类，提供了一种支持可见的用户输入控件模型。
    /// </summary>
    public class TextBox : Control
    {
        #region 成员

        protected float _focusPosition;
        private readonly float _charSize;
        private String _text = "";
        private float _time;
        private Texture2D _focus;
        private bool _isShowFocus;
        private Color _focusColor = Color.Black;
        private Color _fontColor = Color.Black;
        private float _fouceSpeed = 0.6F;
        private SpriteFont _font;
        //左移和右移的保存列表
        private Queue<Char> _leftMoveCollection;
        private Queue<Char> _rightMoveCollection;
        //用来计算当前光标在第几个字符后面
        private int _charOffset = -1;

        /// <summary>
        ///     获取或设置文本框内容
        /// </summary>
        public String Text
        {
            get { return _text; }
            set { _text = value; }
        }
        /// <summary>
        ///     获取或设置光标焦点纹理
        /// </summary>
        public Texture2D Focus
        {
            get { return _focus; }
            set { _focus = value; }
        }
        /// <summary>
        ///     获取或设置光标焦点颜色
        /// </summary>
        public Color FocusColor
        {
            get { return _focusColor; }
            set { _focusColor = value; }
        }
        /// <summary>
        ///     光标焦点闪烁的速度
        /// </summary>
        public float FouceSpeed
        {
            get { return _fouceSpeed; }
            set { _fouceSpeed = value; }
        }
        /// <summary>
        ///     获取或设置文本框字体
        /// </summary>
        public SpriteFont Font
        {
            get { return _font; }
            set { _font = value; }
        }
        /// <summary>
        ///     获取或设置字体颜色
        /// </summary>
        public Color FontColor
        {
            get { return _fontColor; }
            set { _fontColor = value; }
        }

        #endregion

        #region 构造函数

        /// <summary>
        ///      文本框父类，提供了一种支持可见的用户输入控件模型。
        /// </summary>
        /// <param name="game">游戏对象</param>
        /// <param name="area">文本框区域</param>
        /// <param name="charSize">字符大小</param>
        public TextBox(Microsoft.Xna.Framework.Game game, Rectangle area, float charSize)
            : base(game)
        {
            _charSize = charSize;
            _area = area;
            _width = _area.Width;
            _height = _area.Height;
            _getfocus = true;
            KeyDown += TextBoxKeyDown;
        }

        #endregion

        #region 事件

        //按键事件
        void TextBoxKeyDown(Object sender, EventArgs.KeyDownEventArgs e)
        {
            //越界检测
            //MoveFocus(true);
            switch (e.KeyCode)
            {
                //光标左移
                case "left":
                    MoveFocus(true);
                    return;
                //光标右移
                case "right":
                    MoveFocus(false);
                    return;
            }
            _text += e.KeyCode;
            _charOffset++;
            Debug.Print("通过Key Click, 光标位移是：" + _charOffset);
            _focusPosition = _font.MeasureString(_text).X;
            bool isOverRect = (int)_focusPosition > _area.Width;
            if (isOverRect)
            {
                ProcessStoreChar();
            }
            if (_font != null)
            {
                _focusPosition = _font.MeasureString(_text).X;
                Debug.WriteLine("输入字符后的光标检测 : " + _focusPosition);
            }
        }

        #endregion

        #region 方法

        /// <summary>
        ///     移动焦点
        /// </summary>
        /// <param name="isLeftMove">鼠标左移标示位</param>
        protected virtual void MoveFocus(bool isLeftMove)
        {
            if (isLeftMove)
            {
                if (_focusPosition + _position.X <= _position.X)
                {
                    ProcessMoveChar(true);
                    _focusPosition = 0;
                    _charOffset = -1;
                }
                else
                {
                    //不属于截断字符串散列的条件
                    if (_focusPosition <= _width)
                    {
                        if (_charOffset >= 0)
                        {
                            //光标左移
                            _focusPosition -= _font.MeasureString(_text[_charOffset <= 0 ? 0 : --_charOffset].ToString()).X;
                            Debug.Print("光标左移后的Offset : "  + _charOffset);
                        }
                        Debug.WriteLine(_focusPosition);
                    }
                }
            }
            else
            {
                if (_focusPosition >= _area.Width || _charOffset < _text.Length)
                {
                    ProcessMoveChar(false);
                    _focusPosition = _position.X + _area.Width;
                    _charOffset = _text.Length;
                }
                else
                {
                    //不属于截断字符串散列的条件
                    if (_focusPosition <= _width)
                    {
                        if (_charOffset + 1 >= _text.Length)
                        {
                            return;
                        }
                        if (_charOffset < _text.Length)
                        {
                            //光标右移
                            _focusPosition += _font.MeasureString(_text[_charOffset + 1 >= _text.Length ? _text.Length - 1 : ++_charOffset].ToString()).X;
                            Debug.Print("光标右移后的Offset : " + _charOffset);
                        }
                        Debug.WriteLine(_focusPosition);
                    }
                }
            }
        }

        /// <summary>
        ///     处理移动字符
        /// </summary>
        /// <param name="isLeftMove">鼠标左移标示位</param>
        protected void ProcessMoveChar(bool isLeftMove)
        {
            //左移
            if (isLeftMove)
            {
                if (_leftMoveCollection.Count > 0)
                {
                    _text = _leftMoveCollection.Dequeue() + _text;
                    while (_font.MeasureString(_text).X > _area.Width)
                    {
                        char last = _text[_text.Length - 1];
                        _rightMoveCollection.Enqueue(last);
                        _text = _text.Substring(0, _text.Length - 1);
                    }
                }
            }
            else
            //右移
            {
                if (_rightMoveCollection.Count > 0)
                {
                    _text += _rightMoveCollection.Dequeue();
                    while (_font.MeasureString(_text).X > _area.Width)
                    {
                        char first = _text[0];
                        _leftMoveCollection.Enqueue(first);
                        _text = _text.Substring(1, _text.Length - 2);
                    }
                }
            }
        }

        /// <summary>
        ///     用来储存多余的字符
        /// </summary>
        protected void ProcessStoreChar()
        {
            while (_font.MeasureString(_text).X > _area.Width)
            {
                char first = _text[0];
                _text = _text.Substring(1, _text.Length - 1);
                _leftMoveCollection.Enqueue(first);
                _charOffset--;
            }
            Debug.WriteLine(_font.MeasureString(_text).X);
            _focusPosition = _font.MeasureString(_text).X;
        }

        #endregion

        #region 父类方法

        /// <summary>
        ///     Called when the DrawableGameComponent needs to be drawn.  Override this method with component-specific drawing code. Reference page contains links to related conceptual articles.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Microsoft.Xna.Framework.DrawableGameComponent.Draw(Microsoft.Xna.Framework.GameTime).</param>
        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            if (_font != null)
            {
                _spriteBatch.DrawString(_font, _text, _position, _fontColor);
            }
            //如果当前获得焦点
            if (_getfocus && _focus != null)
            {
                if (!_isShowFocus)
                {
                    _spriteBatch.Draw(_focus, 
                                      new Rectangle(
                                              (int) (_position.X + _focusPosition),
                                              (int) _position.Y,
                                              1, 
                                              _area.Height),
                                      _focusColor);
                }
            }
            _spriteBatch.End();
        }

        /// <summary>
        ///     Called when the GameComponent needs to be updated.  Override this method with component-specific update code.
        /// </summary>
        /// <param name="gameTime">Time elapsed since the last call to Microsoft.Xna.Framework.GameComponent.Update(Microsoft.Xna.Framework.GameTime)</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (_getfocus)
            {
                _time += (float)gameTime.ElapsedGameTime.TotalSeconds;
                while (_time > _fouceSpeed)
                {
                    _time -= _fouceSpeed;
                    _isShowFocus = !_isShowFocus;
                }
            }
        }

        /// <summary>
        ///     Called when graphics resources need to be loaded. Override this method to load any component-specific graphics resources. 
        /// </summary>
        protected override void LoadContent()
        {
            if (_spriteBatch == null)
            {
                _spriteBatch = new SpriteBatch(GraphicsDevice);
            }
            _leftMoveCollection = new Queue<char>();
            _rightMoveCollection = new Queue<char>();
            base.LoadContent();
        }

        #endregion
    }
}