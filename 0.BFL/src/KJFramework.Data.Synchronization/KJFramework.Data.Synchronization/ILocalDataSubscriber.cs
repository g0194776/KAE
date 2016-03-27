using KJFramework.Data.Synchronization.Policies;

namespace KJFramework.Data.Synchronization
{
    /// <summary>
    ///     本地数据订阅者接口
    /// </summary>
    public interface ILocalDataSubscriber : ISubscriber
    {
        /// <summary>
        ///     获取发布者策略
        /// </summary>
        IPublisherPolicy Policy { get; }
        /// <summary>
        ///     关闭订阅者
        ///     <para>* 此操作将会导致关闭与订阅者之间的通信信道</para>
        /// </summary>
        void Close();
        /// <summary>
        ///     发送数据
        /// </summary>
        /// <param name="catalog">分组名称</param>
        /// <param name="key">要发送的KEY数据</param>
        /// <param name="value">要发送的VALUE数据</param>
        /// <returns>返回发送的状态</returns>
        /// <exception cref="System.NullReferenceException">非法数据</exception>
        bool Send(string catalog, byte[] key, byte[] value);
    }
}