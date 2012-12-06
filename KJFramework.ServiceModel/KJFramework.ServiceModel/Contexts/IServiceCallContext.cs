using System;

namespace KJFramework.ServiceModel.Contexts
{
    /// <summary>
    ///     服务契约操作调用上下文，提供了相关的基本属性结构。
    /// </summary>
    public interface IServiceCallContext
    {
        /// <summary>
        ///     获取或设置唯一编号
        /// </summary>
        int Id { get; set; }
        /// <summary>
        ///     获取会话密钥
        /// </summary>
        int SessionId { get; }
        /// <summary>
        ///     获取调用实例
        /// </summary>
        Object Instance { get; }
    }
}