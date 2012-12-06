using System;
using KJFramework.Game.Components.Scenarios;

namespace KJFramework.Game.Components.Actions
{
    /// <summary>
    ///     动作元接口，提供了相关的基本操作。
    /// </summary>
    public interface IAction<TScenario> : IInstallable, IDisposable
        where TScenario : IScenario
    {
        /// <summary>
        ///     获取一个值，该值表示了当前动作是否为默认动作
        /// </summary>
        bool IsDefault { get; }
        /// <summary>
        ///     获取或设置名称
        /// </summary>
        String Name { get; set; }
        /// <summary>
        ///     获取或设置附属属性
        /// </summary>
        Object Tag { get; set; }
        /// <summary>
        ///     添加一个场景
        /// </summary>
        /// <param name="scenario"></param>
        void AddScenario(TScenario scenario);
        /// <summary>
        ///     获取指定场景
        /// </summary>
        TScenario GetScenario(String name);
        /// <summary>
        ///     播放具有指定名称的场景
        /// </summary>
        /// <param name="scenarioName">场景名称</param>
        void Play(String scenarioName);
        /// <summary>
        ///     开始动作事件
        /// </summary>
        event EventHandler BeginAction;
        /// <summary>
        ///     结束动作事件
        /// </summary>
        event EventHandler EndAction;
    }
}