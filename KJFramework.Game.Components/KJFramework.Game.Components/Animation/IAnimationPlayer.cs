using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KJFramework.Game.Components.Animation
{
    /// <summary>
    ///     ����������Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IAnimationPlayer
    {
        /// <summary>
        ///     ��ȡ��ǰҪ���ŵĶ���
        /// </summary>
        Animation Animation { get; }
        /// <summary>
        ///     ��ȡ��ǰҪ���Ŷ�����֡��
        /// </summary>
        int FrameIndex { get; }
        /// <summary>
        ///     ��ȡ��ǰ����֡��ƫ��
        /// </summary>
        Vector2 Origin { get; }
        /// <summary>
        ///     ���Ŷ���
        /// </summary>
        void PlayAnimation(Animation animation);
    }
}