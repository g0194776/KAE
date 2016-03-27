using System;
using System.Collections.Generic;
using KJFramework.Messages.Attributes;

namespace KJFramework.Messages.Extends.Contexts
{
    /// <summary>
    ///     消息字段构造动作上下文对象元接口，提供了相关的基本操作。
    /// </summary>
    public interface IFieldBuildActionContext<T> : IDisposable
    {
        /// <summary>
        ///     获取具有指定编号的消息字段上下文
        /// </summary>
        /// <param name="id">字段编号</param>
        /// <returns>返回消息字段上下文</returns>
        KeyValuePair<IIntellectProperty, T>? GetValue(int id);
        /// <summary>
        ///     获取具有指定编号的消息字段上下文
        /// </summary>
        /// <param name="tag">字段附属名称</param>
        /// <returns>返回消息字段上下文</returns>
        KeyValuePair<IIntellectProperty, T>? GetValue(String tag);
    }
}