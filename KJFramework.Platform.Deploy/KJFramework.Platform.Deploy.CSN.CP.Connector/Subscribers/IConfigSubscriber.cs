using System;
using KJFramework.Platform.Deploy.CSN.CP.Connector.SubscribeObjs;
using KJFramework.Platform.Deploy.CSN.ProtocolStack;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector.Subscribers
{
    /// <summary>
    ///     ���ö�Լ��Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IConfigSubscriber
    {
        /// <summary>
        ///     ��ȡ������һ��ֵ����ֵ��ʾ�˵�ǰ�û��Ƿ���Ҫ��̬����ˢ�����õ�����
        /// </summary>
        bool NeedUpdate { get; set; }
        /// <summary>
        ///     ��ȡ�����ö�Լ�˵Ĺؼ�����ֵ
        /// </summary>
        string SubscriberKey { get; }
        /// <summary>
        ///     ��ȡ������Ԥ��Լ����ص�����ͨ�����
        /// </summary>
        Guid ChannelId { get; set; }
        /// <summary>
        ///     �����ö����߷�����Ϣ
        /// </summary>
        /// <param name="message">���͵���Ϣ</param>
        void Send(CSNMessage message);
        /// <summary>
        ///     ȡ����ǰ�����ߵ����ж�����Ϣ
        /// </summary>
        void Cancel();
        /// <summary>
        ///     ��ȡ���Ķ���
        /// </summary>
        /// <typeparam name="T">���Ķ�������</typeparam>
        /// <returns>���ض��Ķ���</returns>
        T GetSubscribeObject<T>() where T : ISubscribeObject;
        /// <summary>
        ///     ������ȡ���¼�
        /// </summary>
        event EventHandler SubscribeCanceled;
    }
}