using Microsoft.Xna.Framework.Graphics;

namespace KJFramework.Game.Components.Animation
{
    /// <summary>
    ///     动画元接口，提供了的基本属性结构
    /// </summary>
    public interface IAnimation
    {
        /// <summary>
        ///     获取动画纹理资源
        /// </summary>
        Texture2D Texture { get; }
        /// <summary>
        ///     获取播放每一帧的时间间隔
        /// </summary>
        float FrameTime { get; }
        /// <summary>
        ///    获取一个值，该值表示了当播放到最后一帧的时候，是否自动从头开始播放。
        /// </summary>
        bool IsLooping { get; }
        /// <summary>
        ///     获取帧数
        /// </summary>
        int FrameCount { get; set; }
        /// <summary>
        ///     获取帧图像宽度
        /// </summary>
        int FrameWidth { get; }
        /// <summary>
        ///    获取帧图像高度
        /// </summary>
        int FrameHeight { get; }
    }
}