using System;
using KJFramework.Data.Synchronization.EventArgs;
using KJFramework.EventArgs;

namespace KJFramework.ApplicationEngine.Proxies
{
    /// <summary>
    ///     远程数据同步代理器
    ///     <para>* 用于简化与远程Data Publisher之间的通信交互</para>
    /// </summary>
    public interface IRemoteDataSyncProxy
    {
        /// <summary>
        ///     创建一个数据订阅者，并且订阅到远程Data Publisher上
        /// </summary>
        /// <param name="catalog">Data Publisher上的分类</param>
        /// <param name="callback">回调函数</param>
        /// <param name="isAutoReconnect">连接断开后的重连标识</param>
        void Regist(string catalog, EventHandler<LightSingleArgEventArgs<DataRecvEventArgs<string, string>>> callback, bool isAutoReconnect = false);
        /// <summary>
        ///     创建一个数据订阅者，并且订阅到远程Data Publisher上
        /// </summary>
        /// <typeparam name="K">Key类型</typeparam>
        /// <typeparam name="V">Value类型</typeparam>
        /// <param name="catalog">Data Publisher上的分类</param>
        /// <param name="iep">远程地址</param>
        /// <param name="callback">回调函数</param>
        /// <param name="isAutoReconnect">连接断开后的重连标识</param>
        void Regist<K, V>(string catalog, string iep, EventHandler<LightSingleArgEventArgs<DataRecvEventArgs<K, V>>> callback, bool isAutoReconnect = false);
    }
}