using LoadRunner;

namespace KJFramework.Test.LoadRunner
{
    /// <summary>
    ///     测试上下文接口
    /// </summary>
    public interface ITestContext
    {
        /// <summary>
        ///     获取LoadRunner API
        /// </summary>
        LrApi Api { get; }
        /// <summary>
        ///     添加一个共享资源
        /// </summary>
        /// <param name="key">KEY</param>
        /// <param name="value">VALUE</param>
        void AddResource(string key, object value);
        /// <summary>
        ///     移除一个共享资源
        /// </summary>
        /// <param name="key">KEY</param>
        void RemoveResource(string key);
        /// <summary>
        ///     获取一个共享资源
        /// </summary>
        /// <param name="key">KEY</param>
        /// <returns>返回获取到的共享资源</returns>
        object GetResource(string key);
    }
}