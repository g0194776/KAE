using System;

namespace KJFramework.Net
{
    /// <summary>
    ///     功能通道元接口，提供了相关的基本操作
    /// </summary>
    public interface IFunctionChannel
    {
        /// <summary>
        ///     获取唯一标识
        /// </summary>
        Guid Key { get; }
        /// <summary>
        ///     处理指定对象，并返回处理后的结果
        /// </summary>
        /// <param name="obj">处理的对象</param>
        /// <param name="isSuccess">是否处理成功的标示</param>
        /// <returns>回处理后的结果</returns>
        object Process(object obj, out bool isSuccess);
    }

    /// <summary>
    ///     功能通道元接口，提供了相关的基本操作
    /// </summary>
    /// <typeparam name="T">处理的对象类型</typeparam>
    public interface IFunctionChannel<T>
    {
        /// <summary>
        ///     获取唯一标识
        /// </summary>
        Guid Key { get; }
        /// <summary>
        ///     处理指定对象，并返回处理后的结果
        /// </summary>
        /// <param name="obj">处理的对象</param>
        /// <param name="isSuccess">是否处理成功的标示</param>
        /// <returns>回处理后的结果</returns>
        T Process(T obj, out bool isSuccess);
    }
}