using System;
using System.Collections.Generic;
using KJFramework.Game.Components.Scenarios;

namespace KJFramework.Game.Components.Actions
{
    /// <summary>
    ///     ������װ��Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IActionInstaller<TScenario>
        where TScenario : Scenario
    {
        /// <summary>
        ///     ��ȡ����ָ����ŵĶ���
        /// </summary>
        /// <param name="name">��������</param>
        /// <returns>���ض���</returns>
        IAction<TScenario> this[String name] { get; }
        /// <summary>
        ///     ��ȡĬ�ϵĶ���
        /// </summary>
        /// <returns>����Ĭ�ϵĶ���</returns>
        IAction<TScenario> GetDefaultAction();
        /// <summary>
        ///     ��ȡ��������
        /// </summary>
        /// <returns>���ض�������</returns>
        IEnumerable<IAction<TScenario>> GetActions();
        /// <summary>
        ///     ��װ����
        /// </summary>
        /// <param name="action">��������</param>
        /// <returns>���ذ�װ��״̬</returns>
        bool Install(IAction<TScenario> action);
        /// <summary>
        ///     ж�ض���
        /// </summary>
        /// <param name="name">��������</param>
        /// <returns>����ж�ص�״̬</returns>
        bool UnInstall(String name);
    }
}