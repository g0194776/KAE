using System;
namespace KJFramework.Logger.LogObject
{
    /// <summary>
    ///     文字日志项格式元接口，提供了相关的基本操作。
    /// </summary>
    public interface ITextLogFormatter
    {
        /// <summary>
        ///    获取上分割符
        /// </summary>
        String Up { get; }
        /// <summary>
        ///    获取上分割符
        /// </summary>
        String Down { get; }
        /// <summary>
        ///     获取左侧分隔符
        /// </summary>
        String LeftSplitChar { get; }
    }
}