using KJFramework.Messages.Contracts;
using KJFramework.Net;

namespace KJFramework.Data.Synchronization
{
    /// <summary>
    ///     系统资源池接口
    /// </summary>
    internal interface ISystemResourcePool
    {
        /// <summary>
        ///     添加一个网络资源
        /// </summary>
        /// <param name="res">网络资源</param>
        /// <returns>返回添加后的状态</returns>
        /// <exception cref="System.NullReferenceException">无效参数</exception>
        PublisherResourceStub Regist(INetworkResource res);
        /// <summary>
        ///     通过一个网络资源获取一个通信信道
        ///     <para>* 如果具有指定条件的信道不存在，将会创建该通信信道</para>
        /// </summary>
        /// <param name="res">网络资源</param>
        /// <returns>返回通信信道</returns>
        /// <exception cref="System.NullReferenceException">无效参数</exception>
        IMessageTransportChannel<MetadataContainer> GetChannel(INetworkResource res);
    }
}