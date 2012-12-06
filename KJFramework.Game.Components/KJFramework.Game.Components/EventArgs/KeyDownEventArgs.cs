using System;
using KJFramework.Game.Basic.Enums;
using Microsoft.Xna.Framework.Input;
namespace KJFramework.Game.Components.EventArgs
{
    /// <summary>
    ///     按键事件
    /// </summary>
    public class KeyDownEventArgs : System.EventArgs
    {
        #region 成员

        private String _keyCode;
        private Keys _key;
        private KeyboardInputAreaTypes _areaType;
        /// <summary>
        ///     获取按下的键码
        /// </summary>
        public String KeyCode
        {
            get { return _keyCode; }
        }
        /// <summary>
        ///     获取按下的键枚举
        /// </summary>
        public Keys Key
        {
            get { return _key; }
        }
        /// <summary>
        ///     获取键盘输入区域
        /// </summary>
        public KeyboardInputAreaTypes AreaType
        {
            get { return _areaType; }
        }

        #endregion

        #region 构造函数

        /// <summary>
        ///     按键事件
        /// </summary>
        /// <param name="key">键枚举</param>
        /// <param name="isCapsLock">是否开启了键盘大写</param>
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