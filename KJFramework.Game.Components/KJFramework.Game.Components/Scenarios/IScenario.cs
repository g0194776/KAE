using System;

namespace KJFramework.Game.Components.Scenarios
{
    /// <summary>
    ///     场景元接口，提供了相关的基本操作。
    /// </summary>
    public interface IScenario : ILoopable, IDisposable
    {
        /// <summary>
        ///     获取一个值，该值标示了当前长久是否为默认场景。
        /// </summary>
        bool IsDefault { get; }
        /// <summary>
        ///     获取场景唯一编号
        /// </summary>
        int Id { get; }
    }
}