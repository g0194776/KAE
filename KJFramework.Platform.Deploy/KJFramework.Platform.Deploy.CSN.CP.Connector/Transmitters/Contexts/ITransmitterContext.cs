using KJFramework.Platform.Deploy.CSN.CP.Connector.Objects;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Subscribers;
using KJFramework.Platform.Deploy.CSN.ProtocolStack;
using KJFramework.Platform.Deploy.CSN.ProtocolStack.Enums;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector.Transmitters.Contexts
{
    /// <summary>
    ///     传输器上下文元接口，提供了相关的基本操作。
    /// </summary>
    public interface ITransmitterContext
    {
        /// <summary>
        ///     获取或设置任务编号
        /// </summary>
        int TaskId { get; set; }
        /// <summary>
        ///     获取或设置总共的数据长度
        /// </summary>
        int TotalDataLength { get; set; }
        /// <summary>
        ///     获取或设置总共的分包数量
        /// </summary>
        int TotalPackageCount { get; set; }
        /// <summary>
        ///     获取或设置上一个请求消息的会话编号
        /// </summary>
        int PreviousSessionId { get; set; }
        /// <summary>
        ///     获取或设置分包数据集合
        /// </summary>
        DataPart[] Datas { get; set; }
        /// <summary>
        ///     获取或设置配置订阅者
        /// </summary>
        IConfigSubscriber Subscriber { get; set; }
        /// <summary>
        ///     获取或设置回馈消息
        /// </summary>
        CSNMessage ResponseMessage { get; set; }
        /// <summary>
        ///     获取或设置配置类型
        /// </summary>
        ConfigTypes ConfigType { get; set; }
        /// <summary>
        ///     获取具有指定关键值的值
        /// </summary>
        /// <typeparam name="T">返回值的类型</typeparam>
        /// <param name="key">关键字</param>
        /// <returns>返回值</returns>
        T Get<T>(string key);
        /// <summary>
        ///     添加一个值
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">要添加的对象</param>
        void Add(string key, object value);
        /// <summary>
        ///     移除一个具有指定关键字的值
        /// </summary>
        /// <param name="key">关键字</param>
        void Remove(string key);
    }
}