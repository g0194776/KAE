namespace KJFramework.ApplicationEngine.Processors
{
    /// <summary>
    ///    KAE消息处理器接口
    /// </summary>
    public interface IKAEProcessor<T>
    {
        #region Methods.

        /// <summary>
        ///    处理一个网络请求
        /// </summary>
        /// <param name="request">请求消息</param>
        /// <returns>返回该网络事务的应答消息</returns>
        T Process(T request);

        #endregion
    }
}