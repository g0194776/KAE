using System;
using System.Collections.Generic;
using KJFramework.Logger.LogObject;

namespace KJFramework.Logger
{
    /// <summary>
    ///     多条数据记录器元接口, 提供了对于多条记录同时存在的相关操作支持。
    /// </summary>
    public interface IMultiItemLogger<I> : ILogger<I> where I : ILog
    {
        /// <summary>
        ///     获取当前所有的记录项
        /// </summary>
        /// <returns>
        ///     返回null, 表示不存在记录项
        /// </returns>
        List<I> GetLog();
        /// <summary>
        ///     获取在指定时间范围内的异常记录
        /// </summary>
        /// <param name="startTime" type="System.DateTime">
        ///     <para>
        ///         起始时间
        ///     </para>
        /// </param>
        /// <param name="endTime" type="System.DateTime">
        ///     <para>
        ///         结束时间
        ///     </para>
        /// </param>
        /// <returns>
        ///     返回null, 表示不存在指定时间范围内的异常。
        /// </returns>
        List<I> GetLog(DateTime startTime, DateTime endTime);
    }
}
