using System;
using KJFramework.Results;

namespace KJFramework.Diagnostics
{
    /// <summary>
    ///     收集器元接口，提供了相关的基本操作。
    /// </summary>
    public interface ICollector : IControlable, IDisposable
    {
        /// <summary>
        ///     获取唯一标示
        /// </summary>
        Guid Id { get; }
        /// <summary>
        ///     获取一个值，该值表示了当前收集器是否可用
        /// </summary>
        bool IsActive { get; }
        event EventHandler BeginWork, EndWork;
    }
}