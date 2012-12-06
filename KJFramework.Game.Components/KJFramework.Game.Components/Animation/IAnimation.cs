using Microsoft.Xna.Framework.Graphics;

namespace KJFramework.Game.Components.Animation
{
    /// <summary>
    ///     ����Ԫ�ӿڣ��ṩ�˵Ļ������Խṹ
    /// </summary>
    public interface IAnimation
    {
        /// <summary>
        ///     ��ȡ����������Դ
        /// </summary>
        Texture2D Texture { get; }
        /// <summary>
        ///     ��ȡ����ÿһ֡��ʱ����
        /// </summary>
        float FrameTime { get; }
        /// <summary>
        ///    ��ȡһ��ֵ����ֵ��ʾ�˵����ŵ����һ֡��ʱ���Ƿ��Զ���ͷ��ʼ���š�
        /// </summary>
        bool IsLooping { get; }
        /// <summary>
        ///     ��ȡ֡��
        /// </summary>
        int FrameCount { get; set; }
        /// <summary>
        ///     ��ȡ֡ͼ����
        /// </summary>
        int FrameWidth { get; }
        /// <summary>
        ///    ��ȡ֡ͼ��߶�
        /// </summary>
        int FrameHeight { get; }
    }
}