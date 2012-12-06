using KJFramework.Game.Components.Controls;
using Microsoft.Xna.Framework;

namespace KJFramework.Game.Components.Objects
{
    /// <summary>
    ///     描述一个窗体控件的最小元素
    /// </summary>
    public class WindowControlPair
    {
        /// <summary>
        ///     获取或设置控件偏移
        /// </summary>
        public Vector2 Position { get; set; }
        /// <summary>
        ///     获取或设置控件
        /// </summary>
        public Control Control { get; set; }
    }
}