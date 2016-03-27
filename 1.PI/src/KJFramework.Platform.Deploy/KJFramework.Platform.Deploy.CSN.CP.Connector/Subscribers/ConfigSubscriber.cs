using System;
using KJFramework.Logger;
using KJFramework.Platform.Deploy.CSN.CP.Connector.SubscribeObjs;
using KJFramework.Platform.Deploy.CSN.ProtocolStack;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector.Subscribers
{
    /// <summary>
    ///     配置订阅者父类，提供了相关的基本操作。
    /// </summary>
    public class ConfigSubscriber: IConfigSubscriber
    {
        #region Constructor

        /// <summary>
        ///     配置订阅者父类，提供了相关的基本操作。
        /// </summary>
        /// <param name="subscriberKey">订阅者唯一序列值</param>
        /// <param name="channelId">订阅者网络通道编号</param>
        public ConfigSubscriber(string subscriberKey, Guid channelId)
        {
            _subscriberKey = subscriberKey;
            _channelId = channelId;
        }

        #endregion

        #region Implementation of IConfigSubscriber<T>

        protected bool _needUpdate;
        protected string _subscriberKey;
        protected Guid _channelId;
        protected ISubscribeObject _subscribeObject;

        /// <summary>
        ///     获取或设置一个值，该值标示了当前用户是否需要动态反向刷新配置的能力
        /// </summary>
        public bool NeedUpdate
        {
            get { return _needUpdate; }
            set { _needUpdate = value; }
        }

        /// <summary>
        ///     获取或设置订约人的关键序列值
        /// </summary>
        public string SubscriberKey
        {
            get { return _subscriberKey; }
        }

        /// <summary>
        ///     获取或设置预订约人相关的网络通道编号
        /// </summary>
        public Guid ChannelId
        {
            get { return _channelId; }
            set { _channelId = value; }
        }

        /// <summary>
        ///     向配置订阅者发送消息
        /// </summary>
        /// <param name="message">发送的消息</param>
        public void Send(CSNMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }
            message.Bind();
            if (!message.IsBind)
            {
                Logs.Logger.Log("Can not send a csn message to dynamic service, because the property of IntellectObject.IsBind is false");
                return;
            }
            Global.ClientNode.Send(_channelId , message.Body);
        }

        /// <summary>
        ///     取消当前订阅者的所有订阅信息
        /// </summary>
        public void Cancel()
        {
            try
            {
                ConfigSubscriberManager.UnRegist(_subscriberKey);
            }
            catch (System.Exception e)
            {
                Logs.Logger.Log(e);
            }
            SubscribeCanceledHandler(null);
        }

        /// <summary>
        ///     获取订阅对象
        /// </summary>
        /// <typeparam name="T">订阅对象类型</typeparam>
        /// <returns>返回订阅对象</returns>
        public T GetSubscribeObject<T>() where T : ISubscribeObject
        {
            return (T) _subscribeObject;
        }

        /// <summary>
        ///     订阅已取消事件
        /// </summary>
        public event EventHandler SubscribeCanceled;
        private void SubscribeCanceledHandler(System.EventArgs e)
        {
            EventHandler handler = SubscribeCanceled;
            if (handler != null) handler(this, e);
        }

        #endregion
    }
}