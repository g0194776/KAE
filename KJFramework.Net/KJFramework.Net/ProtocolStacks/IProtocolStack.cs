using System;
using System.Collections.Generic;
using KJFramework.Net.Exception;

namespace KJFramework.Net.ProtocolStacks
{
    /// <summary>
    ///     协议栈元接口，提供了相关的基本操作。
    /// </summary>
    /// <typeparam name="T">协议栈中父类消息类型</typeparam>
    public interface IProtocolStack<T> : IDisposable
    {
        /// <summary>
        ///     初始化
        /// </summary>
        /// <returns>返回初始化的结果</returns>
        /// <exception cref="InitializeFailedException">初始化失败</exception>
        bool Initialize();
        /// <summary>
        ///     解析元数据
        /// </summary>
        /// <param name="data">元数据</param>
        /// <returns>返回能否解析的一个标示</returns>
        List<T> Parse(byte[] data);
        /// <summary>
        ///     解析元数据
        /// </summary>
        /// <param name="data">总BUFF长度</param>
        /// <param name="offset">可用偏移量</param>
        /// <param name="count">可用长度</param>
        /// <returns>返回能否解析的一个标示</returns>
        List<T> Parse(byte[] data, int offset, int count);
        /// <summary>
        ///     将一个消息转换为2进制形式
        /// </summary>
        /// <param name="message">需要转换的消息</param>
        /// <returns>返回转换后的2进制</returns>
        byte[] ConvertToBytes(T message);
        /// <summary>
        ///     将一个消息转换为多个分包二进制数据
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="maxSize">封包片最大容量</param>
        /// <returns>返回转换后的2进制集合</returns>
        List<byte[]> ConvertMultiMessage(T message, int maxSize);
    }
}