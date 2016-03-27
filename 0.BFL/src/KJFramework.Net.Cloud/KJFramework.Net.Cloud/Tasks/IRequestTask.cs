using System;
using KJFramework.EventArgs;
using KJFramework.Net.Cloud.Nodes;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Net.Channels;
using KJFramework.Tasks;

namespace KJFramework.Net.Cloud.Tasks
{
    /// <summary>
    ///     ��������Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    /// <typeparam name="T">Э��ջ�и�����Ϣ���͡�</typeparam>
    public interface IRequestTask<T> : ITask
    {
        /// <summary>
        ///     ��ȡ��������Ϣ
        /// </summary>
        T Message { get; set; }
        /// <summary>
        ///     ��ȡִ�н��
        /// </summary>
        T ResultMessage { get; }
        /// <summary>
        ///     ��ȡ������һ��ֵ����ֵ��ʾ�˵�ǰ�����Ƿ��Ѿ�������
        /// </summary>
        bool HasRented { get; set; }
        /// <summary>
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰ�������Ƿ��Ѿ���ʱ
        /// </summary>
        bool IsTimeout { get; }
        /// <summary>
        ///     ��ȡ�����ö�Ӧ�Ĵ���ͨ��
        /// </summary>
        IMessageTransportChannel<T> Channel { get; set; }
        /// <summary>
        ///     ��ȡ����Ψһ��ʾ
        /// </summary>
        Guid TaskId { get; }
        /// <summary>
        ///     ��ȡ�����ö�Ӧ������ڵ�
        /// </summary>
        NetworkNode<T> Node { get; set; }
        /// <summary>
        ///     ��ȡ�����ù��ܴ�����
        /// </summary>
        IFunctionProcessor<T> Processor { get; set; }
        /// <summary>
        ///     ���õ�ǰ���������״̬
        /// </summary>
        void Reset();
        /// <summary>
        ///     ����ʱ�¼�
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<T>> ExecuteTimeout;
    }
}