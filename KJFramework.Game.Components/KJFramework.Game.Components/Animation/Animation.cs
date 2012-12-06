using Microsoft.Xna.Framework.Graphics;

namespace KJFramework.Game.Components.Animation
{
    /// <summary>
    ///     一个由水平方向帧资源图片组成的动画
    /// </summary>
    public class Animation : IAnimation
    {
        #region 私有成员

        private Texture2D _texture;
        private float _frameTime;
        private bool _isLooping;

        #endregion

        #region 公共成员

        /// <summary>
        ///     获取动画纹理资源
        /// </summary>
        public Texture2D Texture
        {
            get { return _texture; }
        }

        /// <summary>
        ///     获取播放每一帧的时间间隔
        /// </summary>
        public float FrameTime
        {
            get { return _frameTime; }
        }

        /// <summary>
        ///    获取一个值，该值表示了当播放到最后一帧的时候，是否自动从头开始播放。
        /// </summary>
        public bool IsLooping
        {
            get { return _isLooping; }
        }

        private int _frameCount= - 1;
        /// <summary>
        ///     获取帧数
        /// </summary>
        public int FrameCount
        {
            get { return _frameCount == -1 ? Texture.Width / FrameWidth : _frameCount; }
            set{ _frameCount = value;}
        }

        /// <summary>
        ///     获取帧图像宽度
        /// </summary>
        public int FrameWidth
        {
            // Assume square frames.
            get { return Texture.Height; }
        }

        /// <summary>
        ///    获取帧图像高度
        /// </summary>
        public int FrameHeight
        {
            get { return Texture.Height; }
        }

        #endregion

        #region 构造函数

        /// <summary>
        ///     一个由水平方向帧资源图片组成的动画
        /// </summary>
        /// <param name="texture">动画纹理资源</param>
        /// <param name="frameTime">帧时间间隔</param>
        /// <param name="isLooping">循环播放标志位</param>
        public Animation(Texture2D texture, float frameTime, bool isLooping)
        {
            _texture = texture;
            _frameTime = frameTime;
            _isLooping = isLooping;
        }

        #endregion
    }
}