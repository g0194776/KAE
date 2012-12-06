using System;
using KJFramework.Game.Basic.Enums;
using Microsoft.Xna.Framework.Input;
namespace KJFramework.Game.Components.EventArgs
{
    /// <summary>
    ///     �����¼�
    /// </summary>
    public class KeyDownEventArgs : System.EventArgs
    {
        #region ��Ա

        private String _keyCode;
        private Keys _key;
        private KeyboardInputAreaTypes _areaType;
        /// <summary>
        ///     ��ȡ���µļ���
        /// </summary>
        public String KeyCode
        {
            get { return _keyCode; }
        }
        /// <summary>
        ///     ��ȡ���µļ�ö��
        /// </summary>
        public Keys Key
        {
            get { return _key; }
        }
        /// <summary>
        ///     ��ȡ������������
        /// </summary>
        public KeyboardInputAreaTypes AreaType
        {
            get { return _areaType; }
        }

        #endregion

        #region ���캯��

        /// <summary>
        ///     �����¼�
        /// </summary>
        /// <param name="key">��ö��</param>
        /// <param name="isCapsLock">�Ƿ����˼��̴�д</param>
        public KeyDownEventArgs(Keys key, bool isCapsLock)
        {
            _key = key;
            String value = isCapsLock ? key.ToString() : key.ToString().ToLower();
            if (value.ToLower().StartsWith("d") && value.Length > 1)
            {
                _areaType = KeyboardInputAreaTypes.MainArea;
                _keyCode = value.Replace("d","");
            }
            else if (value.ToLower().StartsWith("numpad"))
            {
                _areaType = KeyboardInputAreaTypes.NumPad;
                _keyCode = value.Replace("numpad", "");
            }
            else if (value.ToLower() == "space")
            {
                _keyCode = " ";
            }
            else
            {
                _keyCode = value;
            }
        }

        #endregion
    }
}