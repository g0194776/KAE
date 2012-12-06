using KJFramework.Platform.Deploy.CSN.CP.Connector.Enums;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Subscribers;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Transmitters.Contexts;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector.Transmitters.Steps
{
    /// <summary>
    ///     传输步骤元接口，提供了相关的基本操作。
    /// </summary>
    public interface ITransmitteStep
    {
        /// <summary>
        ///     执行一个操作
        /// </summary>
        /// <param name="configSubscriber">配置订阅者</param>
        /// <param name="context">上下文</param>
        /// <param name="args">相关参数</param>
        /// <returns>返回执行后的状态</returns>
        TransmitterSteps Do(IConfigSubscriber configSubscriber, ITransmitterContext context, params object[] args);
    }
}