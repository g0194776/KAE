using KJFramework.Net.Channels;
using KJFramework.ServiceModel.Bussiness.Default.Messages;
using KJFramework.ServiceModel.Bussiness.Default.Services;
using KJFramework.ServiceModel.Bussiness.Default.Transactions;
using KJFramework.Statistics;

namespace KJFramework.ServiceModel.Bussiness.Default.Dispatchers.Cores
{
    /// <summary>
    ///     核心分发器，提供了相关的基本操作。
    /// </summary>
    internal interface ICoreDispatcher : IStatisticable<IStatistic>
    {
        /// <summary>
        ///     分发请求
        /// </summary>
        /// <param name="serviceHandle">服务句柄</param>
        /// <param name="message">服务消息</param>
        /// <param name="channel">底层传输通道</param>
        void Dispatch(IServiceHandle serviceHandle, RPCTransaction rpcTrans);
    }
}