using System;
using System.Collections.Generic;
using KJFramework.Basic.Enum;
using KJFramework.Logger.LogObject;

namespace KJFramework.Logger
{
    /// <summary>
    ///     异常记录器元接口, 提供了相关的基本操作
    /// </summary>
    public interface IDebugLogger<T> : ILogger<T> where T : ILog
    {
        /// <summary>
        ///     使用默认的警告等级来记录错误。
        /// </summary>
        /// <param name="e" type="System.Exception">
        ///     <para>
        ///         异常对象
        ///     </para>
        /// </param>
        void Log(System.Exception e);
        /// <summary>
        ///     使用一个指定的等级来记录错误。
        /// </summary>
        /// <param name="e" type="System.Exception">
        ///     <para>
        ///         异常对象
        ///     </para>
        /// </param>
        /// <param name="grade" type="KJFramework.Basic.Enum.DebugGrade">
        ///     <para>
        ///         错误等级
        ///     </para>
        /// </param>
        void Log(System.Exception e, DebugGrade grade);
        /// <summary>
        ///     获取所有异常记录
        /// </summary>
        /// <returns>返回异常记录列表</returns>
        List<T> GetLog();
        /// <summary>
        ///     获取具有指定异常等级异常记录集合
        /// </summary>
        /// <param name="grade" type="KJFramework.Basic.Enum.DebugGrade">
        ///     <para>
        ///         异常等级
        ///     </para>
        /// </param>
        /// <returns>
        ///     返回null, 表示不存在该等级的异常。
        /// </returns>
        List<T> GetLog(DebugGrade grade);
        /// <summary>
        ///     返回在开始日期到截至日期中存在的异常记录
        /// </summary>
        /// <param name="startTime">开始日期</param>
        /// <param name="endTime">截止日期</param>
        /// <returns>返回异常记录列表</returns>
        List<IDebugLog> GetLog(DateTime startTime, DateTime endTime);
        /// <summary>
        ///     获取在指定时间范围内并且具有指定异常等级的异常记录
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
        /// <param name="grade" type="KJFramework.Basic.Enum.DebugGrade">
        ///     <para>
        ///         异常等级
        ///     </para>
        /// </param>
        /// <returns>
        ///     返回null, 表示不存在指定时间范围内的异常。
        /// </returns>
        List<T> GetLog(DateTime startTime, DateTime endTime, DebugGrade grade);
        /// <summary>
        ///     使用指定的异常等级，将指定文字添加到记录集合中
        /// </summary>
        /// <param name="text">要记录的文字</param>
        /// <param name="grade">异常等级</param>
        void Log(String text, DebugGrade grade);
    }
}
