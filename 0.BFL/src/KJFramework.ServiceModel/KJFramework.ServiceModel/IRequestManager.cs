using KJFramework.Net.Channels.Identities;
using KJFramework.ServiceModel.Metadata;

namespace KJFramework.ServiceModel
{
    /// <summary>
    ///     请求管理器元接口，提供了相关的基本操作
    /// </summary>
    public interface IRequestManager
    {
        /// <summary>
        ///     获取指定会话请求返回的结果
        /// </summary>
        /// <typeparam name="T">返回结果的类型</typeparam>
        /// <param name="identity">事务唯一标识</param>
        /// <param name="isAsync">异步请求标示</param>
        /// <returns>返回值</returns>
        T GetResult<T>(TransactionIdentity identity, bool isAsync);
        /// <summary>
        ///     根据一个会话编号添加一个返回结果
        /// </summary>
        /// <param name="identity">事务唯一标识</param>
        /// <param name="result">返回结果</param>
        void AddResult(TransactionIdentity identity, IBinaryArgContext result);
    }
}