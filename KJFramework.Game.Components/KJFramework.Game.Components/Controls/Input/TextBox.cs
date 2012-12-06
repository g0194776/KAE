using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KJFramework.Game.Components.Controls.Input
{
    /// <summary>
    ///     �ı����࣬�ṩ��һ��֧�ֿɼ����û�����ؼ�ģ�͡�
    /// </summary>
    public class TextBox : Control
    {
        #region ��Ա

        //��ǰ���ƫ��
        protected float _focusPosition;
        //Ԥ֪��ǰ�ı���Ŀ�������ͬʱ��ʾ���ٸ��ַ�
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
        ///     ��ȡ�������ı�������
        /// </summary>
        public String Text
        {
            get { return GetText(); }
            set
            {
                _displayContent = "";
                _text = value;
                //�ոպã�������������ʾ��������
                if (_text.Length <= _maxFontSize)
                {
                    _displayContent = _text;
                    _focusPosition += _charSize;
                }
                //��Ҫ��ȡ��
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
        ///     ��ȡ�����ù�꽹������
        /// </summary>
        public Texture2D Focus
        {
            get { return _focus; }
            set { _focus = value; }
        }
        /// <summary>
        ///     ��ȡ�����ù�꽹����ɫ
        /// </summary>
        public Color FocusColor
        {
            get { return _focusColor; }
            set { _focusColor = value; }
        }
        /// <summary>
        ///     ��꽹����˸���ٶ�
        /// </summary>
        public float FouceSpeed
        {
            get { return _fouceSpeed; }
            set { _fouceSpeed = value; }
        }
        /// <summary>
        ///     ��ȡ�������ı�������
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
        ///     ��ȡ������������ɫ
        /// </summary>
        public Color FontColor
        {
            get { return _fontColor; }
            set { _fontColor = value; }
        }

        /// <summary>
        ///     ��ȡ�������ı��򱳾�ͼƬ
        /// </summary>
        public Texture2D Background
        {
            get { return _background; }
            set { _background = value; }
        }

        #endregion

        #region ���캯��

        /// <summary>
        ///      �ı����࣬�ṩ��һ��֧�ֿɼ����û�����ؼ�ģ�͡�
        /// </summary>
        /// <param name="game">��Ϸ����</param>
        /// <param name="area">�ı�������</param>
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

        #region �¼�

        //�����¼�
        void TextBoxKeyDown(Object sender, EventArgs.KeyDownEventArgs e)
        {
            //Խ����
            switch (e.KeyCode)
            {
                //�������
                case "left":
                    MoveFocus(true);
                    return;
                //�������
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
            //�ոպã�������������ʾ��������
            if (_text.Length <= _maxFontSize)
            {
                _displayContent = _text;
                _focusPosition += _charSize;
            }
            //��Ҫ��ȡ��
            else
            {
                string leftAppend = _text.Substring(0, _text.Length - _maxFontSize);
                foreach (char c in leftAppend)
                {
                    _leftStore.Enqueue(c);
                }
                _displayContent = _text.Substring(_text.Length - _maxFontSize);
            }
            Debug.Print("���յ��������룺" + e.KeyCode);
            Debug.Print("�ܹ��洢���룺" + _text);
        }

        #endregion

        #region ����

        /// <summary>
        ///     �ƶ�����
        /// </summary>
        /// <param name="isLeftMove">������Ʊ�ʾλ</param>
        protected virtual void MoveFocus(bool isLeftMove)
        {
            //�������߽�
            if (isLeftMove && _focusPosition == 0 && !String.IsNullOrEmpty(_displayContent))
            {
                //��ǰ����û��ʾ�����ݣ��������洢���л�������
                if (_text.Length - _maxFontSize > 0 && _leftStore.Count > 0)
                {
                    _rightStore.Enqueue(_displayContent[_displayContent.Length - 1]);
                    _displayContent = _leftStore.Dequeue() + _displayContent.Substring(0, _displayContent.Length - 1);
                }
                return;
            }
            //������ұ߽�
            if (!isLeftMove && _focusPosition >= _maxFontSize * _charSize)
            {
                //��ǰ����û��ʾ�����ݣ������Ҳ�洢���л�������
                if (_text.Length - _maxFontSize > 0 && _rightStore.Count > 0)
                {
                    _leftStore.Enqueue(_displayContent[0]);
                    _displayContent = _displayContent.Substring(1, _displayContent.Length - 1) + _rightStore.Dequeue();
                }
                return;
            }
            //��ʼ�ƶ����(����ܵ������֤����ǣ�������ݵ����⣬ֻ�ǹ����ƶ�����)
            if (isLeftMove)
            {
                //����
                _focusPosition -= _charSize;
            }
            else
            {
                if (!String.IsNullOrEmpty(_displayContent))
                {
                    //����Ҳ໺����Ϊ�գ����ҵ�ǰ����ʾ�����Ѿ�С�ڿ������ʾ������
                    if (_displayContent.Length > (_focusPosition / _charSize) || _rightStore.Count > 0)
                    {
                        //����
                        _focusPosition += _charSize;
                    }
                }
            }
        }

        /// <summary>
        ///     ɾ���ַ�ʱʹ��
        /// </summary>
        protected virtual void DeleteChar()
        {
            //��������Ҳ�
            if ((_focusPosition >= _maxFontSize * _charSize) || (_text.Length != 0 && _text.Length <= _maxFontSize))
            {
                //���洢������δ��ʾ���ַ�));
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
                Debug.Print("��������Ҳ࣬ɾ�������������Ϊ��" + GetText());
            }
        }

        /// <summary>
        ///     ��ȡ��ǰ�ı����е���������
        /// </summary>
        /// <returns>������������</returns>
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

        #region ���෽��

        /// <summary>
        ///     Called when the DrawableGameComponent needs to be drawn.  Override this method with component-specific drawing code. Reference page contains links to related conceptual articles.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Microsoft.Xna.Framework.DrawableGameComponent.Draw(Microsoft.Xna.Framework.GameTime).</param>
        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            //������
            if (_background != null)
            {
                _spriteBatch.Draw(_background, new Rectangle((int) _position.X, (int) _position.Y, (int) _width, (int) _height), Color.White);
            }
            if (_font != null)
            {
                _spriteBatch.DrawString(_font, _displayContent, _position, _fontColor);
            }
            //�����ǰ��ý���
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