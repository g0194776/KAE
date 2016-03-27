using System;
using KJFramework.Logger;
using KJFramework.Platform.Deploy.CSN.CP.Connector.SubscribeObjs;
using KJFramework.Platform.Deploy.CSN.ProtocolStack;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector.Subscribers
{
    /// <summary>
    ///     ���ö����߸��࣬�ṩ����صĻ���������
    /// </summary>
    public class ConfigSubscriber: IConfigSubscriber
    {
        #region Constructor

        /// <summary>
        ///     ���ö����߸��࣬�ṩ����صĻ���������
        /// </summary>
        /// <param name="subscriberKey">������Ψһ����ֵ</param>
        /// <param name="channelId">����������ͨ�����</param>
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
        ///     ��ȡ������һ��ֵ����ֵ��ʾ�˵�ǰ�û��Ƿ���Ҫ��̬����ˢ�����õ�����
        /// </summary>
        public bool NeedUpdate
        {
            get { return _needUpdate; }
            set { _needUpdate = value; }
        }

        /// <summary>
        ///     ��ȡ�����ö�Լ�˵Ĺؼ�����ֵ
        /// </summary>
        public string SubscriberKey
        {
            get { return _subscriberKey; }
        }

        /// <summary>
        ///     ��ȡ������Ԥ��Լ����ص�����ͨ�����
        /// </summary>
        public Guid ChannelId
        {
            get { return _channelId; }
            set { _channelId = value; }
        }

        /// <summary>
        ///     �����ö����߷�����Ϣ
        /// </summary>
        /// <param name="message">���͵���Ϣ</param>
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
        ///     ȡ����ǰ�����ߵ����ж�����Ϣ
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
        ///     ��ȡ���Ķ���
        /// </summary>
        /// <typeparam name="T">���Ķ�������</typeparam>
        /// <returns>���ض��Ķ���</returns>
        public T GetSubscribeObject<T>() where T : ISubscribeObject
        {
            return (T) _subscribeObject;
        }

        /// <summary>
        ///     ������ȡ���¼�
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