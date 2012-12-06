using System;
using KJFramework.Game.Components.Scenarios;

namespace KJFramework.Game.Components.Actions
{
    /// <summary>
    ///     ����Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IAction<TScenario> : IInstallable, IDisposable
        where TScenario : IScenario
    {
        /// <summary>
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰ�����Ƿ�ΪĬ�϶���
        /// </summary>
        bool IsDefault { get; }
        /// <summary>
        ///     ��ȡ����������
        /// </summary>
        String Name { get; set; }
        /// <summary>
        ///     ��ȡ�����ø�������
        /// </summary>
        Object Tag { get; set; }
        /// <summary>
        ///     ���һ������
        /// </summary>
        /// <param name="scenario"></param>
        void AddScenario(TScenario scenario);
        /// <summary>
        ///     ��ȡָ������
        /// </summary>
        TScenario GetScenario(String name);
        /// <summary>
        ///     ���ž���ָ�����Ƶĳ���
        /// </summary>
        /// <param name="scenarioName">��������</param>
        void Play(String scenarioName);
        /// <summary>
        ///     ��ʼ�����¼�
        /// </summary>
        event EventHandler BeginAction;
        /// <summary>
        ///     ���������¼�
        /// </summary>
        event EventHandler EndAction;
    }
}