using System;
using KJFramework.Mono.Game.Components.Controls;
using XnaTouch.Framework;

namespace KJFramework.Mono.Game.Components.Scenarios
{
    /// <summary>
    ///     ����Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IScenario : IGameComponent, ILoopable, IDisposable
    {
        /// <summary>
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰ�����Ƿ�ΪĬ�ϳ�����
        /// </summary>
        bool IsDefault { get; }
        /// <summary>
        ///     ��ȡ����Ψһ���
        /// </summary>
        int Id { get; }
        /// <summary>
        ///     ���һ����Ϸ���
        /// </summary>
        /// <param name="component">��Ϸ���</param>
        void Add(Control component);
    }
}