using System;
using KJFramework.Enums;
using KJFramework.Net.Channels;
using KJFramework.Statistics;

namespace KJFramework.ServiceModel.Elements
{
    /// <summary>
    ///     绑定元接口，提供了相关的基本操作。
    /// </summary>
    public interface IBinding : IDisposable, ICommunicationChannelAddress, IStatisticable<IStatistic>
    {
        /// <summary>
        ///     获取默认的命名空间
        /// </summary>
        String DefaultNamespace { get; }
        /// <summary>
        ///     获取绑定方式
        /// </summary>
        BindingTypes BindingType { get; }
        /// <summary>
        ///     获取一个值，该值标示了当前是否已经初始化成功
        /// </summary>
        bool Initialized { get; }
        /// <summary>
        ///     初始化
        /// </summary>
        void Initialize();
        /// <summary>
        ///     获取所有绑定元素
        /// </summary>
        /// <returns></returns>
        BindingElement<TChannel>[] GetBindingElements<TChannel>() where TChannel : IServiceChannel, new();
    }
}