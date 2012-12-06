using System.Net;
using KJFramework.Data.Synchronization.Enums;

namespace KJFramework.Data.Synchronization
{
    /// <summary>
    ///     网络资源接口
    /// </summary>
    public interface INetworkResource
    {
        /// <summary>
        ///     获取或设置资源类型
        /// </summary>
        ResourceMode Mode { get; }
        /// <summary>
        ///     更换资源类型
        /// </summary>
        /// <param name="port">本地要监听的端口</param>
        void Change(int port);
        /// <summary>
        ///     更换资源类型
        /// </summary>
        /// <param name="iep">
        ///     远程地址
        ///     <para>* 格式: ip:port</para>
        /// </param>
        void Change(string iep);
        /// <summary>
        ///     更换资源类型
        /// </summary>
        /// <param name="iep">远程终结点地址</param>
        void Change(IPEndPoint iep);
        /// <summary>
        ///     获取网络资源
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <returns>返回内部的资源</returns>
        T GetResource<T>();
    }
}