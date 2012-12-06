using KJFramework.Game.Components.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace KJFramework.Game.Components.Events
{
    /// <summary>
    ///     �¼�����Ԫ�ӿڣ��ṩ����صĻ�������
    /// </summary>
    public interface IEventHook
    {
        /// <summary>
        ///     ��ȡ�������ڲ��ؼ�
        /// </summary>
        Control Control { get; set; }
        /// <summary>
        ///     ִ��һ���¼�����
        /// </summary>
        /// <param name="keyboardState">����״̬</param>
        /// <param name="mouseState">���״̬</param>
        /// <param name="gameTime">��Ϸʱ��</param>
        /// <returns>�����Ƿ���Ҫ������һ�����ӵı�ʾλ</returns>
        bool Execute(KeyboardState keyboardState, MouseState mouseState, GameTime gameTime);
    }
}