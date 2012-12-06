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
        //���ƺ����Ƶı����б�
        private Queue<Char> _leftMoveCollection;
        private Queue<Char> _rightMoveCollection;
        //�������㵱ǰ����ڵڼ����ַ�����
        private int _charOffset = -1;

        /// <summary>
        ///     ��ȡ�������ı�������
        /// </summary>
        public String Text
        {
            get { return _text; }
            set { _text = value; }
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
            set { _font = value; }
        }
        /// <summary>
        ///     ��ȡ������������ɫ
        /// </summary>
        public Color FontColor
        {
            get { return _fontColor; }
            set { _fontColor = value; }
        }

        #endregion

        #region ���캯��

        /// <summary>
        ///      �ı����࣬�ṩ��һ��֧�ֿɼ����û�����ؼ�ģ�͡�
        /// </summary>
        /// <param name="game">��Ϸ����</param>
        /// <param name="area">�ı�������</param>
        /// <param name="charSize">�ַ���С</param>
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

        #region �¼�

        //�����¼�
        void TextBoxKeyDown(Object sender, EventArgs.KeyDownEventArgs e)
        {
            //Խ����
            //MoveFocus(true);
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
            }
            _text += e.KeyCode;
            _charOffset++;
            Debug.Print("ͨ��Key Click, ���λ���ǣ�" + _charOffset);
            _focusPosition = _font.MeasureString(_text).X;
            bool isOverRect = (int)_focusPosition > _area.Width;
            if (isOverRect)
            {
                ProcessStoreChar();
            }
            if (_font != null)
            {
                _focusPosition = _font.MeasureString(_text).X;
                Debug.WriteLine("�����ַ���Ĺ���� : " + _focusPosition);
            }
        }

        #endregion

        #region ����

        /// <summary>
        ///     �ƶ�����
        /// </summary>
        /// <param name="isLeftMove">������Ʊ�ʾλ</param>
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
                    //�����ڽض��ַ���ɢ�е�����
                    if (_focusPosition <= _width)
                    {
                        if (_charOffset >= 0)
                        {
                            //�������
                            _focusPosition -= _font.MeasureString(_text[_charOffset <= 0 ? 0 : --_charOffset].ToString()).X;
                            Debug.Print("������ƺ��Offset : "  + _charOffset);
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
                    //�����ڽض��ַ���ɢ�е�����
                    if (_focusPosition <= _width)
                    {
                        if (_charOffset + 1 >= _text.Length)
                        {
                            return;
                        }
                        if (_charOffset < _text.Length)
                        {
                            //�������
                            _focusPosition += _font.MeasureString(_text[_charOffset + 1 >= _text.Length ? _text.Length - 1 : ++_charOffset].ToString()).X;
                            Debug.Print("������ƺ��Offset : " + _charOffset);
                        }
                        Debug.WriteLine(_focusPosition);
                    }
                }
            }
        }

        /// <summary>
        ///     �����ƶ��ַ�
        /// </summary>
        /// <param name="isLeftMove">������Ʊ�ʾλ</param>
        protected void ProcessMoveChar(bool isLeftMove)
        {
            //����
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
            //����
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
        ///     �������������ַ�
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

        #region ���෽��

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
            //�����ǰ��ý���
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