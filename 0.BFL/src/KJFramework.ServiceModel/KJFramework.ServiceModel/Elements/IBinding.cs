using System;
using KJFramework.Enums;
using KJFramework.Net.Channels;
using KJFramework.Statistics;

namespace KJFramework.ServiceModel.Elements
{
    /// <summary>
    ///     ��Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IBinding : IDisposable, ICommunicationChannelAddress, IStatisticable<IStatistic>
    {
        /// <summary>
        ///     ��ȡĬ�ϵ������ռ�
        /// </summary>
        String DefaultNamespace { get; }
        /// <summary>
        ///     ��ȡ�󶨷�ʽ
        /// </summary>
        BindingTypes BindingType { get; }
        /// <summary>
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰ�Ƿ��Ѿ���ʼ���ɹ�
        /// </summary>
        bool Initialized { get; }
        /// <summary>
        ///     ��ʼ��
        /// </summary>
        void Initialize();
        /// <summary>
        ///     ��ȡ���а�Ԫ��
        /// </summary>
        /// <returns></returns>
        BindingElement<TChannel>[] GetBindingElements<TChannel>() where TChannel : IServiceChannel, new();
    }
}