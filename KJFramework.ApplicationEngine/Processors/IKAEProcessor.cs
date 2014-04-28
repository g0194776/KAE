using KJFramework.ApplicationEngine.Packages;
using KJFramework.Net.Transaction;

namespace KJFramework.ApplicationEngine.Processors
{
    /// <summary>
    ///    KAE消息处理器接口
    /// </summary>
    /// <typeparam name="T">网络消息类型</typeparam>
    public interface IKAEProcessor<T>
    {
        #region Methods.

        /// <summary>
        ///    处理一个网络请求
        /// </summary>
        /// <param name="package">消息事务</param>
        void Process(IMessageTransaction<T> package);

        #endregion
    }
}