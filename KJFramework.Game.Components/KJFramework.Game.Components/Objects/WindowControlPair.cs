using KJFramework.Game.Components.Controls;
using Microsoft.Xna.Framework;

namespace KJFramework.Game.Components.Objects
{
    /// <summary>
    ///     ����һ������ؼ�����СԪ��
    /// </summary>
    public class WindowControlPair
    {
        /// <summary>
        ///     ��ȡ�����ÿؼ�ƫ��
        /// </summary>
        public Vector2 Position { get; set; }
        /// <summary>
        ///     ��ȡ�����ÿؼ�
        /// </summary>
        public Control Control { get; set; }
    }
}