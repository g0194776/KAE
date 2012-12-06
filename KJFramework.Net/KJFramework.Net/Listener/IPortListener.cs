
namespace KJFramework.Net.Listener
{
    /// <summary>
    ///     端口监听器元接口, 提供了相关的基本操作。
    /// </summary>
    public interface IPortListener : IListener<int>
    {
        /// <summary>
        ///     监听的端口
        /// </summary>
        int Port { get; set; }
        /// <summary>
        ///     监听
        /// </summary>
        void Listen();
    }
}
