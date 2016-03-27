using System;

namespace KJFramework.Messages.Extends
{
    /// <summary>
    ///     构造动作元接口，提供了相关的基本操作。
    /// </summary>
    public interface IBuildAction : IDisposable
    {
        /// <summary>
        ///     获取构造动作编号
        /// </summary>
        int BuildId { get; }
        /// <summary>
        ///     获取一个值，该值标示了当前构造动作是否正在执行指定操作。
        /// </summary>
        bool IsBuilding { get; }
    }
}