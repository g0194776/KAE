using System;

namespace KJFramework.Net.Cloud.Virtuals.Processors
{
    /// <summary>
    ///     傀儡功能处理器元接口，提供了相关的基本操作
    /// </summary>
    public interface IPuppetFunctionProcessor
    {
        /// <summary>
        ///     获取唯一编号
        /// </summary>
        Guid Id { get; }
        /// <summary>
        ///     初始化
        /// </summary>
        /// <typeparam name="T">宿主类型</typeparam>
        /// <param name="target">宿主对象</param>
        /// <returns>返回初始化的状态</returns>
        bool Initialize<T>(T target);
        /// <summary>
        ///     释放当前的傀儡功能处理
        /// </summary>
        void Release();
    }
}