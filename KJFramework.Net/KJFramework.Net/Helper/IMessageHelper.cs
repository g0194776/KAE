using System;
using KJFramework.Logger.LogObject;
using KJFramework.Logger;

namespace KJFramework.Net.Helper
{
    /// <summary>
    ///     消息帮助器元接口, 提供了相关的基本操作和相关属性结构
    /// </summary>
    public interface IMessageHelper
    {
        /// <summary>
        ///     获取或设置异常记录器
        /// </summary>
        IDebugLogger<IDebugLog> DebugLogger { get; set; }
        /// <summary>
        ///     获取或设置消息头结束标记
        /// </summary>
        String MessageHeaderEndFlag { get; set; }
        /// <summary>
        ///     获取或设置消息头标示
        /// </summary>
        String MessageHeaderFlag { get; set; }
        /// <summary>
        ///     获取或设置消息头标示长度
        /// </summary>
        int MessageHeaderFlagLength { get; set; }
        /// <summary>
        ///     消息头长度
        /// </summary>
        int MessageHeaderLength { get; set; }
    }
}
