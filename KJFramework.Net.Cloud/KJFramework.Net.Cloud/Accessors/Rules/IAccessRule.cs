using System;

namespace KJFramework.Net.Cloud.Accessors.Rules
{
    /// <summary>
    ///     访问规则元接口，提供了相关的基本操作。
    /// </summary>
    public interface IAccessRule : IDisposable
    {
        /// <summary>
        ///     获取一个值，该值标示了当前访问是否被允许
        /// </summary>
        bool Accessed { get; }
    }
}