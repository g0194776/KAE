using System;
using KJFramework.EventArgs;

namespace KJFramework.Hook
{
    /// <summary>
    ///     ��깳��Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IMouseHook : IIOHook
    {
        /// <summary>
        ///     ��װ����
        /// </summary>
        /// <returns>
        ///     ��װ״̬
        /// </returns>
        bool InstallHook();
        /// <summary>
        ///      ж�ع���
        /// </summary>
        void UnInstallHook();
        /// <summary>
        ///     ����ƶ��¼�
        /// </summary>
        event EventHandler<MouseMoveEventArgs> MouseMove;
    }
}