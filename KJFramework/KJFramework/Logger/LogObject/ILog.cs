using System;
using KJFramework.Basic.Enum;

namespace KJFramework.Logger.LogObject
{
    /// <summary>
    ///     记录对象元接口, 提供了被记录对象的基本属性结构。
    /// </summary>
    public interface ILog
    {
        /// <summary>
        ///     获取或设置记录具体日期事件
        /// </summary>
        DateTime Time { get; set; }
        /// <summary>
        ///     获取或设置记录名称
        /// </summary>
        String Name { get; set; }
        /// <summary>
        ///     获取或设置记录人
        /// </summary>
        String User { get; set; }
        /// <summary>
        ///     获取该记录对象的类型
        /// </summary>
        LogTypes LogType { get; }
        /// <summary>
        ///     获取或设置记录辅助数据
        /// </summary>
        String Tag { get; set; }
    }
}
