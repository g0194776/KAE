using System;
using KJFramework.EventArgs;
using KJFramework.Plugin;
using KJFramework.Statistics;

namespace KJFramework.MessageStacks
{
    /// <summary>
    ///     ��ϢЭ��ջԪ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IMessageStack<TMessage> : IStatisticable<IStatistic>,  IPlugin, IDisposable
    {
        /// <summary>
        ///     ��ȡһ������ָ��Э���ŵ���Ϣ
        /// </summary>
        /// <param name="protocolId">Э����</param>
        /// <returns>����ָ����Ϣ</returns>
        TMessage Pickup(int protocolId);
        /// <summary>
        ///      ��ȡһ������ָ��Э�����Լ������ŵ���Ϣ
        /// </summary>
        /// <param name="protocolId">Э����</param>
        /// <param name="serviceId">������</param>
        /// <returns>����ָ����Ϣ</returns>
        TMessage Pickup(int protocolId, int serviceId);
        /// <summary>
        ///      ��ȡһ������ָ��Э���ţ��������Լ���ϸ�����ŵ���Ϣ
        /// </summary>
        /// <param name="protocolId">Э����</param>
        /// <param name="serviceId">������</param>
        /// <param name="detailServiceId">��ϸ������</param>
        /// <returns>����ָ����Ϣ</returns>
        TMessage Pickup(int protocolId, int serviceId, int detailServiceId);
        /// <summary>
        ///     ��ȡ��ǰЭ��ջ�е���Ϣ����
        /// </summary>
        int Count { get; }
        /// <summary>
        ///     ��ȡЭ��ջ����
        /// </summary>
        String Name { get; }
        /// <summary>
        ///     ��ȡЭ��ջ�汾
        /// </summary>
        String Version { get; }
        /// <summary>
        ///     ��ȡ��Ϣ�ɹ��¼�
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<TMessage>> PickupSuccessfully;
        /// <summary>
        ///     ��ȡ��Ϣʧ���¼�
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<int>> PickupFailed;
    }
}