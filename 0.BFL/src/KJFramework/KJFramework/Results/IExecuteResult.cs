using KJFramework.Enums;

namespace KJFramework.Results
{
    /// <summary>
    ///     执行结果接口
    /// </summary>
    public interface IExecuteResult
    {
        /// <summary>
        ///     获取执行结果的状态
        /// </summary>
        ExecuteResults State { get; }
        /// <summary>
        ///     获取内部系统的错误编号
        /// </summary>
        byte ErrorId { get; }
        /// <summary>
        ///     获取错误信息
        /// </summary>
        string Error { get; }
        /// <summary>
        ///     获取内部所包含的调用结果对象
        /// </summary>
        /// <typeparam name="T">调用结果对象的类型</typeparam>
        /// <returns>返回调用结果</returns>
        T GetResult<T>();
    }
}