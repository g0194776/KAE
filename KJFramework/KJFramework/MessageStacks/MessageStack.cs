using System;
using System.Collections.Generic;
using KJFramework.Basic.Enum;
using KJFramework.EventArgs;
using KJFramework.Plugin;
using KJFramework.Statistics;

namespace KJFramework.MessageStacks
{
    /// <summary>
    ///     消息协议栈抽象类，提供了相关的基本操作
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public abstract class MessageStack<TMessage> : IMessageStack<TMessage>
    {
        #region 析构函数

        ~MessageStack()
        {
            Dispose();
        }

        #endregion

        #region Implementation of IStatisticable<IStatistic>

        protected Dictionary<StatisticTypes, IStatistic> _statistics;
        protected int _key;
        protected PluginInfomation _pluginInfo;
        protected bool _enable;
        protected PluginTypes _pluginType;
        protected int _count;
        protected string _name;
        protected string _version;

        /// <summary>
        ///     获取或设置统计器
        /// </summary>
        public Dictionary<StatisticTypes, IStatistic> Statistics
        {
            get { return _statistics; }
            set { _statistics = value; }
        }

        #endregion

        #region Implementation of IMetadata<int>

        /// <summary>
        /// 获取或设置用来约束所有对象的唯一标示
        /// </summary>
        public int Key
        {
            get { return _key; }
            set { _key = value; }
        }

        #endregion

        #region Implementation of IPlugin

        /// <summary>
        ///     获取或设置插件信息
        /// </summary>
        public PluginInfomation PluginInfo
        {
            get { return _pluginInfo; }
        }

        /// <summary>
        ///      获取或设置可用标示
        /// </summary>
        public bool Enable
        {
            get { return _enable; }
            set { _enable = value; }
        }

        /// <summary>
        ///     获取或设置插件类型
        /// </summary>
        public PluginTypes PluginType
        {
            get { return _pluginType; }
        }

        /// <summary>
        ///     加载后需要做的动作
        /// </summary>
        public abstract void OnLoading();

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        ///     注销
        /// </summary>
        public void Dispose()
        {
           GC.SuppressFinalize(this);
        }

        #endregion

        #region Implementation of IMessageStack<TMessage>

        /// <summary>
        ///     提取一个具有指定协议编号的消息
        /// </summary>
        /// <param name="protocolId">协议编号</param>
        /// <returns>返回指定消息</returns>
        public abstract TMessage Pickup(int protocolId);
        /// <summary>
        ///      提取一个具有指定协议编号以及服务编号的消息
        /// </summary>
        /// <param name="protocolId">协议编号</param>
        /// <param name="serviceId">服务编号</param>
        /// <returns>返回指定消息</returns>
        public abstract TMessage Pickup(int protocolId, int serviceId);
        /// <summary>
        ///      提取一个具有指定协议编号，服务编号以及详细服务编号的消息
        /// </summary>
        /// <param name="protocolId">协议编号</param>
        /// <param name="serviceId">服务编号</param>
        /// <param name="detailServiceId">详细服务编号</param>
        /// <returns>返回指定消息</returns>
        public abstract TMessage Pickup(int protocolId, int serviceId, int detailServiceId);

        /// <summary>
        ///     获取当前协议栈中的消息数量
        /// </summary>
        public int Count
        {
            get { return _count; }
        }

        /// <summary>
        ///     获取协议栈名称
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        ///     获取协议栈版本
        /// </summary>
        public string Version
        {
            get { return _version; }
        }

        /// <summary>
        ///     提取消息成功事件
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<TMessage>> PickupSuccessfully;
        protected void PickupSuccessfullyHandler(LightSingleArgEventArgs<TMessage> e)
        {
            EventHandler<LightSingleArgEventArgs<TMessage>> successfully = PickupSuccessfully;
            if (successfully != null) successfully(this, e);
        }

        /// <summary>
        ///     提取消息失败事件
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<int>> PickupFailed;
        protected void PickupFailedHandler(LightSingleArgEventArgs<int> e)
        {
            EventHandler<LightSingleArgEventArgs<int>> failed = PickupFailed;
            if (failed != null) failed(this, e);
        }

        #endregion
    }
}