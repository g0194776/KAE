using System;
using System.Collections.Generic;
using KJFramework.Game.Components.Scenarios;

namespace KJFramework.Game.Components.Actions
{
    /// <summary>
    ///     动作安装器元接口，提供了相关的基本操作。
    /// </summary>
    public interface IActionInstaller<TScenario>
        where TScenario : Scenario
    {
        /// <summary>
        ///     获取具有指定编号的动作
        /// </summary>
        /// <param name="name">场景名称</param>
        /// <returns>返回动作</returns>
        IAction<TScenario> this[String name] { get; }
        /// <summary>
        ///     获取默认的动作
        /// </summary>
        /// <returns>返回默认的动作</returns>
        IAction<TScenario> GetDefaultAction();
        /// <summary>
        ///     获取动作集合
        /// </summary>
        /// <returns>返回动作集合</returns>
        IEnumerable<IAction<TScenario>> GetActions();
        /// <summary>
        ///     安装动作
        /// </summary>
        /// <param name="action">动作对象</param>
        /// <returns>返回安装的状态</returns>
        bool Install(IAction<TScenario> action);
        /// <summary>
        ///     卸载动作
        /// </summary>
        /// <param name="name">场景名称</param>
        /// <returns>返回卸载的状态</returns>
        bool UnInstall(String name);
    }
}