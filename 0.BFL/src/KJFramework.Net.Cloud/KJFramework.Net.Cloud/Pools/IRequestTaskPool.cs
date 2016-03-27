using KJFramework.Net.Cloud.Tasks;
using KJFramework.Net.Exception;

namespace KJFramework.Net.Cloud.Pools
{
    /// <summary>
    ///     请求任务池元接口，提供了相关的基本操作。
    /// </summary>
    /// <typeparam name="T">协议栈中父类消息类型。</typeparam>
    public interface IRequestTaskPool<T>
    {
        /// <summary>
        ///     获取或设置当前所支持的最大任务数量
        /// </summary>
        int MaxCount { get; set; }
        /// <summary>
        ///     初始化
        /// </summary>
        /// <exception cref="InitializeFailedException">初始化失败</exception>
        void Initialzie();
        /// <summary>
        ///     租一个请求任务
        /// </summary>
        /// <returns>
        ///     返回请求任务
        ///     <para>* 如果当前的可用任务为0，则会同步等待。</para>
        /// </returns>
        IRequestTask<T> Rent();
        /// <summary>
        ///     归还一个请求任务
        /// </summary>
        /// <param name="task">任务</param>
        void Giveback(IRequestTask<T> task);
    }
}