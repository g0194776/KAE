using System;
using KJFramework.Net.Exception;

namespace KJFramework.Net.Cloud.Nodes
{
    /// <summary>
    ///     功能节点元接口，提供了相关的基本操作。
    /// </summary>
    /// <typeparam name="T">协议栈中父类消息类型。</typeparam>
    public interface IFunctionNode<T>
    {
        /// <summary>
        ///     获取可用标示
        /// </summary>
        bool Enable { get; }
        /// <summary>
        ///     获取唯一键值
        /// </summary>
        Guid Id { get; }
        /// <summary>
        ///   获取或设置附属属性
        /// </summary>
        Object Tag { get; set; }
        /// <summary>
        ///     初始化
        /// </summary>
        /// <returns>返回初始化状态</returns>
        /// <exception cref="InitializeFailedException">初始化失败</exception>
        bool Initialize();
    }
}