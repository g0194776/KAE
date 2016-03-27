using System;
using KJFramework.Net.Channels;

namespace KJFramework.ServiceModel.Elements
{
    /// <summary>
    ///     绑定元素元接口，提供了相关的基本操作
    /// </summary>
    internal interface IBindingElement<TChannel> : ICommunicationObject
        where TChannel : IServiceChannel
    {
        /// <summary>
        ///     获取绑定元素名称
        /// </summary>
        String Name { get; } 
        /// <summary>
        ///     获取一个值，该值标示了当前绑定元素是否可以绑定
        /// </summary>
        bool CanBind { get; }
        /// <summary>
        ///     获取一个值，该值表示了当前通道监听器是否已经初始化成功
        /// </summary>
        bool Initialized { get; }
        /// <summary>
        ///     初始化
        /// </summary>
        void Initialize();
        /// <summary>
        ///     创建通道
        /// </summary>
        /// <returns>返回创建后的通道</returns>
        TChannel CreateChannel();
    }
}