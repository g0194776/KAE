using System.Net;

namespace KJFramework.Net.Channels
{
    /// <summary>
    ///     通讯通道地址元接口，提供了相关的基本属性结构。
    /// </summary>
    public interface ICommunicationChannelAddress
    {
        /// <summary>
        ///     获取或设置物理地址
        /// </summary>
        IPEndPoint Address { get; set; }
        /// <summary>
        ///     获取或设置逻辑地址
        /// </summary>
        Uri.Uri LogicalAddress { get; set; }
    }
}