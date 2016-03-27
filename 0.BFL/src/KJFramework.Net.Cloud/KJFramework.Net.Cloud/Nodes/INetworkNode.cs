using System;
using KJFramework.EventArgs;
using KJFramework.Net.Cloud.Accessors;
using KJFramework.Net.Cloud.Accessors.Rules;
using KJFramework.Net.Cloud.Exceptions;
using KJFramework.Net.Cloud.Objects;
using KJFramework.Net.ProtocolStacks;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.HostChannels;

namespace KJFramework.Net.Cloud.Nodes
{
    /// <summary>
    ///     ����ڵ�Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    /// <typeparam name="T">Э��ջ�и�����Ϣ���͡�</typeparam>
    public interface INetworkNode<T> : INetServiceNode
    {
        /// <summary>
        ///     ��ȡ�����÷�����
        /// </summary>
        IAccessor Accessor { get; set; }
        /// <summary>
        ///     ��ȡΨһ��ֵ
        /// </summary>
        Guid Id { get; }
        /// <summary>
        ///     ��ȡЭ��ջ
        /// </summary>
        IProtocolStack<T> ProtocolStack { get; }
        /// <summary>
        ///     ��ָ��Ԫ���ݹ㲥�����е�ǰ���е��ŵ��ϡ�
        /// </summary>
        /// <param name="data">��Ҫ�㲥��Ԫ����</param>
        void Broadcast(byte[] data);
        /// <summary>
        ///   ��ָ����Ϣ�㲥�����е�ǰ���е��ŵ��ϡ� 
        /// </summary>
        /// <param name="message">��Ҫ�㲥����Ϣ</param>
        void Broadcast(T message);
        /// <summary>
        ///     ������ǰ����ڵ�
        /// </summary>
        void Open();
        /// <summary>
        ///     �رյ�ǰ����ڵ�
        /// </summary>
        void Close();
        /// <summary>
        ///     ��һ������ͨ���У�ִ�����ӵ�Զ���ս��Ĳ���
        /// </summary>
        /// <param name="channel">����ͨ��</param>
        /// <returns>�������ӵ�״̬</returns>
        bool Connect(IRawTransportChannel channel);
        /// <summary>
        ///     ��ȡһ������ָ��ID�Ĵ���ͨ��
        /// </summary>
        /// <param name="id">ͨ��Ψһ��ʾ</param>
        /// <returns>���ش���ͨ��</returns>
        ITransportChannel GetTransportChannel(Guid id);
        /// <summary>
        ///     ע��һ������ͨ��
        /// </summary>
        /// <param name="channel">����ͨ��</param>
        /// <exception cref="System.ArgumentNullException">��������</exception>
        /// <exception cref="System.Exception">ע��ʧ��</exception>
        void Regist(IHostTransportChannel channel);
        /// <summary>
        ///     ע��һ������ͨ��
        /// </summary>
        /// <param name="id">����ͨ��Ψһ��ʾ</param>
        void UnRegist(Guid id);
        /// <summary>
        ///     ����Ԫ���ݵ�����ָ��ͨ����ŵ�Զ�̽ڵ���
        /// </summary>
        /// <param name="id">����ͨ�����</param>
        /// <param name="data">Ԫ����</param>
        /// <exception cref="TransportChannelNotFoundException">����ͨ��������</exception>
        void Send(Guid id, byte[] data);
        /// <summary>
        ///     ����һ����Ϣ������ָ��ͨ����ŵ�Զ�̽ڵ���
        /// </summary>
        /// <param name="id">����ͨ�����</param>
        /// <param name="message">Ԫ����</param>
        /// <exception cref="TransportChannelNotFoundException">����ͨ��������</exception>
        /// <exception cref="System.ArgumentNullException">��������</exception>
        void Send(Guid id, T message);
        /// <summary>
        ///     ���ܾ��������¼�
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<IAccessRule>> ConnectedButNotAllow;
        /// <summary>
        ///     �����µĴ���ͨ���¼�
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<IRawTransportChannel>> NewTransportChannelCreated;
        /// <summary>
        ///     ���յ�����Ϣ�¼�
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<ReceivedMessageObject<T>>> NewMessageReceived;
        /// <summary>
        ///     ����ͨ�����Ƴ��¼�
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<Guid>> TransportChannelRemoved;
    }
}