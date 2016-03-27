using System;
using KJFramework.Net.Cloud.Nodes;
using KJFramework.Net.Cloud.Objects;
using KJFramework.Net.Channels;
using KJFramework.Statistics;

namespace KJFramework.Net.Cloud.Schedulers
{
    /// <summary>
    ///     ���������Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    /// <typeparam name="T">Э��ջ�и�����Ϣ���͡�</typeparam>
    public interface IRequestScheduler<T> : IStatisticable<IStatistic>
    {
        /// <summary>
        ///     ��ȡΨһ��ʾ
        /// </summary>
        Guid Id { get; }
        /// <summary>
        ///   ���յ�ǰ�Ѿ�ע��Ĺ��ܽڵ㣬���ȴ���һ����Ϣ����
        /// </summary>
        /// <param name="networkNode">����ڵ�</param>
        /// <param name="target">���յ�����Ϣ����</param>
        void Schedule(NetworkNode<T> networkNode,  ReceivedMessageObject<T> target);
        /// <summary>
        ///     ���յ�ǰ�Ѿ�ע��Ĺ��ܽڵ㣬���ȴ���һ����Ϣ����
        /// </summary>
        /// <param name="message">��Ϣ</param>
        /// <param name="networkNode">����ڵ�</param>
        /// <param name="channel">����ͨ��</param>
        void Schedule(T message, NetworkNode<T> networkNode, IMessageTransportChannel<T> channel);
        /// <summary>
        ///     ע������ڵ�
        /// </summary>
        /// <param name="node">����ڵ�</param>
        void Regist(INetworkNode<T> node);
        /// <summary>
        ///     ע�Ṧ�ܽڵ�
        /// </summary>
        /// <param name="node">���ܽڵ�</param>
        void Regist(IMessageFunctionNode<T> node);
        /// <summary>
        ///     ע������ڵ�
        /// </summary>
        /// <param name="id">Ψһ��ʾ</param>
        void UnRegistNetworkNode(Guid id);
        /// <summary>
        ///     ע�Ṧ�ܽڵ�
        /// </summary>
        /// <param name="id">Ψһ��ʾ</param>
        void UnRegistFunctionNode(Guid id);
        /// <summary>
        ///     ��ʼ����
        /// </summary>
        void Start();
        /// <summary>
        ///     ֹͣ����
        /// </summary>
        void Stop();

    }
}