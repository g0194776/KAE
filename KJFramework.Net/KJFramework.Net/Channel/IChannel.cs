namespace KJFramework.Net.Channel
{
    /// <summary>
    ///     通道元接口，提供了相关的基本操作
    /// </summary>
    /// <typeparam name="TInfomation">通道信息类型</typeparam>
    public interface IChannel<TInfomation>
        where TInfomation : BasicChannelInfomation, new()
    {
        /// <summary>
        ///     获取或设置当前通道信息
        /// </summary>
        TInfomation ChannelInfo { get; set; }
    }
}