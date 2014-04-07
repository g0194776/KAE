using KJFramework.ApplicationEngine.Packages;

namespace KJFramework.ApplicationEngine.Processors
{
    /// <summary>
    ///    KAE消息处理器
    /// </summary>
    /// <typeparam name="T">网络消息类型</typeparam>
    public interface IKAEProcessor<T>
    {
        #region Methods.

        /// <summary>
        ///    处理一个网络请求
        /// </summary>
        /// <param name="package">业务包裹</param>
        void Process(IBusinessPackage<T> package);

        #endregion
    }
}