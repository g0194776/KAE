using System;

namespace KJFramework.Game.Components.Scenarios
{
    /// <summary>
    ///     ����Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IScenario : ILoopable, IDisposable
    {
        /// <summary>
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰ�����Ƿ�ΪĬ�ϳ�����
        /// </summary>
        bool IsDefault { get; }
        /// <summary>
        ///     ��ȡ����Ψһ���
        /// </summary>
        int Id { get; }
    }
}