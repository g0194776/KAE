using System;

namespace KJFramework.Net.Listener
{
    /// <summary>
    ///     端口监听器详细信息元接口，提供了基本信息。
    /// </summary>
    public interface IPortListenerInfomation
    {
        /// <summary>
        ///     获取或设置监听器唯一ID
        ///         * 可以使用Listener.GetHashCode()获得
        /// </summary>
        int ListenerID { get; set;}
        /// <summary>
        ///     获取或设置监听器分组ID
        /// </summary>
        int ItemID { get; set;}
        /// <summary>
        ///     获取或设置监听器服务ID
        /// </summary>
        int ServiceID { get; set;}
        /// <summary>
        ///     获取或设置附属属性
        /// </summary>
        Object Tag { get; set;}
    }
}
