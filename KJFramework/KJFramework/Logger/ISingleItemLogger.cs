using KJFramework.Logger.LogObject;

namespace KJFramework.Logger
{
    /// <summary>
    ///     单一数据记录器元接口, 提供了对于单一记录的相关操作支持。
    /// </summary>
    public interface ISingleItemLogger<T> : ILogger<T> where T : ILog
    {
        /// <summary>
        ///     获取记录
        /// </summary>
        /// <returns>
        ///     返回null, 表示不存在。
        /// </returns>
        T GetLog();
        /// <summary>
        ///     使用新的记录替换现有的记录
        /// </summary>
        /// <param name="log" type="T">
        ///     <para>
        ///         新纪录
        ///     </para>
        /// </param>
        void Instead(T log);
    }
}
