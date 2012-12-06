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

        //当前光标偏移
        protected float _focusPosition;
        //预知当前文本框的宽度最多能同时显示多少个字符
        protected int _maxFontSize;
        private String _text = "";
        private float _time;
        private Texture2D _focus;
        private bool _isShowFocus;
        private Color _focusColor = Color.Black;
        private Color _fontColor = Color.Black;
        private float _fouceSpeed = 0.6F;
        private SpriteFont _font;
        private float _charSize;
        private String _displayContent = "";
        private Queue<char> _leftStore = new Queue<char>();
        private Queue<char> _rightStore = new Queue<char>();
        private Texture2D _background;

        /// <summary>
        ///     获取或设置文本框内容
        /// </summary>
        public String Text
        {
            get { return GetText(); }
            set
            {
                _displayContent = "";
                _text = value;
                //刚刚好，还可以完整显示所有内容
                if (_text.Length <= _maxFontSize)
                {
                    _displayContent = _text;
                    _focusPosition += _charSize;
                }
                //需要截取了
                else
                {
                    string leftAppend = _text.Substring(0, _text.Length - _maxFontSize);
                    foreach (char c in leftAppend)
                    {
                        _leftStore.Enqueue(c);
                    }
                    _displayContent = _text.Substring(_text.Length - _maxFontSize);
                }
            }
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
            set
            { 
                _font = value;
                if (_font != null)
                {
                    _charSize = _font.MeasureString("0").X;
                    _maxFontSize = (int) (_width/_charSize);
                }
            }
        }
        /// <summary>
        ///     获取或设置字体颜色
        /// </summary>
        public Color FontColor
        {
            get { return _fontColor; }
            set { _fontColor = value; }
        }

        /// <summary>
        ///     获取或设置文本框背景图片
        /// </summary>
        public Texture2D Background
        {
            get { return _background; }
            set { _background = value; }
        }

        #endregion

        #region 构造函数

        /// <summary>
        ///      文本框父类，提供了一种支持可见的用户输入控件模型。
        /// </summary>
        /// <param name="game">游戏对象</param>
        /// <param name="area">文本框区域</param>
        public TextBox(Microsoft.Xna.Framework.Game game, Rectangle area)
            : base(game)
        {
            _area = area;
            _width = _area.Width;
            _height = _area.Height;
            Getfocus = true;
            KeyDown += TextBoxKeyDown;
            _isShowFocus = true;
        }

        #endregion

        #region 事件

        //按键事件
        void TextBoxKeyDown(Object sender, EventArgs.KeyDownEventArgs e)
        {
            //越界检测
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
                case "back":
                    DeleteChar();
                    return;
                case "f1":
                case "f2":
                case "f3":
                case "f4":
                case "f5":
                case "f6":
                case "f7":
                case "f8":
                case "f9":
                case "f10":
                case "f11":
                case "f12":
                case "leftshift":
                case "rightshift":
                case "oemtilde":
                case "leftcontrol":
                case "rightcontrol":
                case "CapsLock":
                case "Tab":
                case "printscreen":
                case "insert":
                case "elete":
                case "escape":
                case " ":
                case "up":
                case "own":
                case "enter":
                case "pagedown":
                case "pageup":
                case "home":
                case "rightalt":
                case "leftalt":
                case "apps":
                case "leftwindows":
                    return;
            }
            _text += e.KeyCode;
            //刚刚好，还可以完整显示所有内容
            if (_text.Length <= _maxFontSize)
            {
                _displayContent = _text;
                _focusPosition += _charSize;
            }
            //需要截取了
            else
            {
                string leftAppend = _text.Substring(0, _text.Length - _maxFontSize);
                foreach (char c in leftAppend)
                {
                    _leftStore.Enqueue(c);
                }
                _displayContent = _text.Substring(_text.Length - _maxFontSize);
            }
            Debug.Print("接收到键盘输入：" + e.KeyCode);
            Debug.Print("总共存储输入：" + _text);
        }

        #endregion

        #region 方法

        /// <summary>
        ///     移动焦点
        /// </summary>
        /// <param name="isLeftMove">鼠标左移标示位</param>
        protected virtual void MoveFocus(bool isLeftMove)
        {
            //检测光标左边界
            if (isLeftMove && _focusPosition == 0 && !String.IsNullOrEmpty(_displayContent))
            {
                //当前还有没显示的内容，并且左侧存储队列还有内容
                if (_text.Length - _maxFontSize > 0 && _leftStore.Count > 0)
                {
                    _rightStore.Enqueue(_displayContent[_displayContent.Length - 1]);
                    _displayContent = _leftStore.Dequeue() + _displayContent.Substring(0, _displayContent.Length - 1);
                }
                return;
            }
            //检测光标右边界
            if (!isLeftMove && _focusPosition >= _maxFontSize * _charSize)
            {
                //当前还有没显示的内容，并且右侧存储队列还有内容
                if (_text.Length - _maxFontSize > 0 && _rightStore.Count > 0)
                {
                    _leftStore.Enqueue(_displayContent[0]);
                    _displayContent = _displayContent.Substring(1, _displayContent.Length - 1) + _rightStore.Dequeue();
                }
                return;
            }
            //开始移动光标(如果能到这里，则证明不牵扯到内容的问题，只是光标的移动而已)
            if (isLeftMove)
            {
                //左移
                _focusPosition -= _charSize;
            }
            else
            {
                if (!String.IsNullOrEmpty(_displayContent))
                {
                    //如果右侧缓存器为空，并且当前的显示内容已经小于可最大显示的字数
                    if (_displayContent.Length > (_focusPosition / _charSize) || _rightStore.Count > 0)
                    {
                        //右移
                        _focusPosition += _charSize;
                    }
                }
            }
        }

        /// <summary>
        ///     删除字符时使用
        /// </summary>
        protected virtual void DeleteChar()
        {
            //光标在最右侧
            if ((_focusPosition >= _maxFontSize * _charSize) || (_text.Length != 0 && _text.Length <= _maxFontSize))
            {
                //左侧存储器还有未显示的字符));
                if (_leftStore.Count > 0)
                {
                    _displayContent = _leftStore.Dequeue() + _displayContent.Substring(0, _displayContent.Length - 1);
                }
                else
                {
                    _displayContent = _displayContent.Substring(0, _displayContent.Length - 1);
                    _focusPosition -= _charSize;
                }
                _text = _text.Substring(0, _text.Length - 1);
                Debug.Print("光标在最右侧，删除后的完整内容为：" + GetText());
            }
        }

        /// <summary>
        ///     获取当前文本框中的所有内容
        /// </summary>
        /// <returns>返回所有内容</returns>
        protected virtual String GetText()
        {
            String newText = "";
            if (_leftStore.Count > 0)
            {
                char[] lefts = new char[_leftStore.Count];
                for (int i = 0; i < lefts.Length; i++)
                {
                    lefts[i] = _leftStore.Dequeue();
                    newText += lefts[i];
                }
                for (int i = 0; i < lefts.Length; i++)
                {
                    _leftStore.Enqueue(lefts[i]);
                }
            }
            if (!String.IsNullOrEmpty(_displayContent))
            {
                newText += _displayContent;
            }
            if (_rightStore.Count > 0)
            {
                char[] rights = new char[_rightStore.Count];
                for (int i = 0; i < rights.Length; i++)
                {
                    rights[i] = _rightStore.Dequeue();
                    newText += rights[i];
                }
                for (int i = 0; i < rights.Length; i++)
                {
                    _rightStore.Enqueue(rights[i]);
                }
            }
            return newText;
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
            //画背景
            if (_background != null)
            {
                _spriteBatch.Draw(_background, new Rectangle((int) _position.X, (int) _position.Y, (int) _width, (int) _height), Color.White);
            }
            if (_font != null)
            {
                _spriteBatch.DrawString(_font, _displayContent, _position, _fontColor);
            }
            //如果当前获得焦点
            if (Getfocus && _focus != null)
            {
                if (_isShowFocus)
                {
                    _spriteBatch.Draw(_focus, 
                                      new Rectangle(
                                              (int) (_position.X + _focusPosition),
                                              (int) _position.Y,
                                              1, 
                                              (int) _height),
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
            if (Getfocus)
            {
                _time += (float)gameTime.ElapsedGameTime.TotalSeconds;
                while (_time > _fouceSpeed)
                {
                    _time -= _fouceSpeed;
                    _isShowFocus = !_isShowFocus;
                }
            }
        }

        #endregion
    }
}