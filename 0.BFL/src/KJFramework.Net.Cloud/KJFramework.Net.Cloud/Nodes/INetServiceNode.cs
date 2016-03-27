using System;
using KJFramework.ServiceModel.Elements;
using Uri = KJFramework.Net.Channels.Uri.Uri;

namespace KJFramework.Net.Cloud.Nodes
{
    /// <summary>
    ///     网络服务节点源接口，提供了相关的基本操作。
    /// </summary>
    public interface INetServiceNode
    {
        /// <summary>
        ///     连接到一个远程的服务节点
        /// </summary>
        /// <param name="binding">服务地址绑定对象</param>
        /// <returns>返回连接后的客户端代理</returns>
        T Connect<T>(Binding binding) where T : class;
        /// <summary>
        ///     获取远程服务代理
        /// </summary>
        /// <typeparam name="T">远程服务契约类型</typeparam>
        /// <param name="uri">远程服务地址</param>
        /// <returns>返回客户端代理</returns>
        T GetService<T>(Uri uri) where T : class;
        /// <summary>
        ///     注册一个远程服务
        /// </summary>
        /// <param name="binding">绑定方式</param>
        /// <param name="type">契约类型</param>
        void Regist(Binding binding, Type type);
        /// <summary>
        ///     注销一个远程服务
        /// </summary>
        /// <param name="uri">调用地址</param>
        void UnRegist(Uri uri);
    }
}