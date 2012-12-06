using Microsoft.Xna.Framework.Graphics;

namespace KJFramework.Game.Components.Animation
{
    /// <summary>
    ///     һ����ˮƽ����֡��ԴͼƬ��ɵĶ���
    /// </summary>
    public class Animation : IAnimation
    {
        #region ˽�г�Ա

        private Texture2D _texture;
        private float _frameTime;
        private bool _isLooping;

        #endregion

        #region ������Ա

        /// <summary>
        ///     ��ȡ����������Դ
        /// </summary>
        public Texture2D Texture
        {
            get { return _texture; }
        }

        /// <summary>
        ///     ��ȡ����ÿһ֡��ʱ����
        /// </summary>
        public float FrameTime
        {
            get { return _frameTime; }
        }

        /// <summary>
        ///    ��ȡһ��ֵ����ֵ��ʾ�˵����ŵ����һ֡��ʱ���Ƿ��Զ���ͷ��ʼ���š�
        /// </summary>
        public bool IsLooping
        {
            get { return _isLooping; }
        }

        private int _frameCount= - 1;
        /// <summary>
        ///     ��ȡ֡��
        /// </summary>
        public int FrameCount
        {
            get { return _frameCount == -1 ? Texture.Width / FrameWidth : _frameCount; }
            set{ _frameCount = value;}
        }

        /// <summary>
        ///     ��ȡ֡ͼ����
        /// </summary>
        public int FrameWidth
        {
            // Assume square frames.
            get { return Texture.Height; }
        }

        /// <summary>
        ///    ��ȡ֡ͼ��߶�
        /// </summary>
        public int FrameHeight
        {
            get { return Texture.Height; }
        }

        #endregion

        #region ���캯��

        /// <summary>
        ///     һ����ˮƽ����֡��ԴͼƬ��ɵĶ���
        /// </summary>
        /// <param name="texture">����������Դ</param>
        /// <param name="frameTime">֡ʱ����</param>
        /// <param name="isLooping">ѭ�����ű�־λ</param>
        public Animation(Texture2D texture, float frameTime, bool isLooping)
        {
            _texture = texture;
            _frameTime = frameTime;
            _isLooping = isLooping;
        }

        #endregion
    }
}