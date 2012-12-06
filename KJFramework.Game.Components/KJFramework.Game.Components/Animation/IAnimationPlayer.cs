using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KJFramework.Game.Components.Animation
{
    /// <summary>
    ///     动画播放器元接口，提供了相关的基本操作。
    /// </summary>
    public interface IAnimationPlayer
    {
        /// <summary>
        ///     获取当前要播放的动画
        /// </summary>
        Animation Animation { get; }
        /// <summary>
        ///     获取当前要播放动画的帧数
        /// </summary>
        int FrameIndex { get; }
        /// <summary>
        ///     获取当前动画帧的偏移
        /// </summary>
        Vector2 Origin { get; }
        /// <summary>
        ///     播放动画
        /// </summary>
        void PlayAnimation(Animation animation);
    }
}