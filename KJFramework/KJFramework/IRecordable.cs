using KJFramework.Logger;
using KJFramework.Logger.LogObject;

namespace KJFramework
{
    /// <summary>
    ///     可记录的元接口，提供了与记录相关的基本操作。
    /// </summary>
    /// <typeparam name="TLogger">记录器类型</typeparam>
    /// <typeparam name="TLog">记录项类型</typeparam>
    public interface IRecordable<TLogger, TLog>
        where TLog : ILog
        where TLogger : ILogger<TLog>
    {
        /// <summary>
        ///     获取或设置记录器
        /// </summary>
        TLogger Logger { get; set; }
    }
}