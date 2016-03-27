using System;
using KJFramework.EventArgs;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Enums;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Subscribers;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Transmitters.Contexts;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Transmitters.Steps;
using KJFramework.Platform.Deploy.CSN.ProtocolStack.Enums;
using KJFramework.Statistics;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector.Transmitters
{
    /// <summary>
    ///     配置传输器元接口，提供了相关的基本操作。
    /// </summary>
    public interface IConfigTransmitter : IStatisticable<IStatistic>
    {
        /// <summary>
        ///     获取一个关联此传输器的唯一任务编号
        /// </summary>
        int TaskId { get; }
        /// <summary>
        ///     获取或设置传输器上下文
        /// </summary>
        ITransmitterContext Context { get; set; }
        /// <summary>
        ///     获取配置订阅者
        /// </summary>
        IConfigSubscriber Subscriber { get; }
        /// <summary>
        ///     获取或设置下一个传输步骤的类型
        /// </summary>
        TransmitterSteps NextStep { get; set; }
        /// <summary>
        ///     获取配置类型
        /// </summary>
        ConfigTypes ConfigType { get; }
        /// <summary>
        ///     获取或设置上一次产生动作的时间
        /// </summary>
        DateTime LastActionTime { get; set; }
        /// <summary>
        ///     注册一个传输器步骤
        /// </summary>
        /// <param name="transmitterStep">传输器步骤枚举</param>
        /// <param name="step">传输器步骤</param>
        void Regist(TransmitterSteps transmitterStep, ITransmitteStep step);
        /// <summary>
        ///     执行下一步传输任务
        /// </summary>
        void Next(params object[] args);
        /// <summary>
        ///     当前进度的发布事件
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<string>> Processing;
    }
}