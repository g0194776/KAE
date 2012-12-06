using System;
using KJFramework.Mono.Game.Components.Controls;
using XnaTouch.Framework;

namespace KJFramework.Mono.Game.Components.Scenarios
{
    /// <summary>
    ///     场景元接口，提供了相关的基本操作。
    /// </summary>
    public interface IScenario : IGameComponent, ILoopable, IDisposable
    {
        /// <summary>
        ///     获取一个值，该值标示了当前长久是否为默认场景。
        /// </summary>
        bool IsDefault { get; }
        /// <summary>
        ///     获取场景唯一编号
        /// </summary>
        int Id { get; }
        /// <summary>
        ///     添加一个游戏组件
        /// </summary>
        /// <param name="component">游戏组件</param>
        void Add(Control component);
    }
}