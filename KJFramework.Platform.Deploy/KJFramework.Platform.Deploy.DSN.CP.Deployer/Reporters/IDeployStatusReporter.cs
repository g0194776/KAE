using System;

namespace KJFramework.Platform.Deploy.DSN.CP.Deployer.Reporters
{
    /// <summary>
    ///     部署状态汇报器元接口，提供了相关的基本操作。
    /// </summary>
    public interface IDeployStatusReporter
    {
        /// <summary>
        ///     获取与此汇报器相关联的通道编号
        /// </summary>
        Guid ChannelId { get; }
        /// <summary>
        ///     获取与此汇报器相关联的请求令牌
        /// </summary>
        string RequestToken { get; }
        /// <summary>
        ///     将一个状态通知到远程终结点
        /// </summary>
        /// <param name="content">状态信息</param>
        /// <exception cref="System.Exception">通知失败</exception>
        void Notify(string content);
        /// <summary>
        ///     将一个错误信息通知到远程终结点
        /// </summary>
        /// <param name="exception">异常</param>
        /// <exception cref="System.Exception">通知失败</exception>
        void Notify(System.Exception exception);
    }
}