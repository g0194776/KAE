using KJFramework.Basic;
using KJFramework.Logger.LogObject;
using KJFramework.Logger;

namespace KJFramework.Net.Listener
{
    /// <summary>
    ///     监听器元接口, 提供了相关的基本操作
    /// </summary>
    public interface IListener<TKeyType> : IMetadata<TKeyType>, IControlable
    {
        /// <summary>
        ///     获取或设置异常记录器
        /// </summary>
        IDebugLogger<IDebugLog> DebugLogger { get; set; }
        /// <summary>
        ///     获取或设置当前的状态
        /// </summary>
        bool State { get; set; }
    }
}
