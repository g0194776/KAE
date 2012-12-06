using Microsoft.Xna.Framework;

namespace KJFramework.Game.Components
{
    /// <summary>
    ///     ��ѭ������ϷԪ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface ILoopable
    {
        /// <summary>
        ///     ����
        /// </summary>
        /// <param name="gameTime">��Ϸʱ��</param>
        void Draw(GameTime gameTime);
        /// <summary>
        ///     ����
        /// </summary>
        /// <param name="gameTime">��Ϸʱ��</param>
        void Update(GameTime gameTime);
    }
}