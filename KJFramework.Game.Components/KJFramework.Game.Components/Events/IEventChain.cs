using KJFramework.Game.Components.Controls;
using Microsoft.Xna.Framework;

namespace KJFramework.Game.Components.Events
{
    /// <summary>
    ///     �¼���Ԫ�ӿڣ��ṩ����صĻ�������
    /// </summary>
    public interface IEventChain
    {
        /// <summary>
        ///     ��ȡ�ڲ��Ŀؼ�
        /// </summary>
        Control Control { get; }
        /// <summary>
        ///     ע��һ���¼�����
        /// </summary>
        /// <param name="eventHook">�¼�����</param>
        /// <returns>��������</returns>
        IEventChain Regist(IEventHook eventHook);
        /// <summary>
        ///     ִ��
        /// </summary>
        /// <param name="gameTime">��Ϸʱ��</param>
        void Execute(GameTime gameTime);
    }
}