using System;
using KJFramework.EventArgs;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Enums;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Subscribers;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Transmitters.Contexts;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Transmitters.Steps;
using KJFramework.Platform.Deploy.CSN.ProtocolStack.Enums;
using KJFramework.Statistics;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector.Transmitters
{
    /// <summary>
    ///     ���ô�����Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IConfigTransmitter : IStatisticable<IStatistic>
    {
        /// <summary>
        ///     ��ȡһ�������˴�������Ψһ������
        /// </summary>
        int TaskId { get; }
        /// <summary>
        ///     ��ȡ�����ô�����������
        /// </summary>
        ITransmitterContext Context { get; set; }
        /// <summary>
        ///     ��ȡ���ö�����
        /// </summary>
        IConfigSubscriber Subscriber { get; }
        /// <summary>
        ///     ��ȡ��������һ�����䲽�������
        /// </summary>
        TransmitterSteps NextStep { get; set; }
        /// <summary>
        ///     ��ȡ��������
        /// </summary>
        ConfigTypes ConfigType { get; }
        /// <summary>
        ///     ��ȡ��������һ�β���������ʱ��
        /// </summary>
        DateTime LastActionTime { get; set; }
        /// <summary>
        ///     ע��һ������������
        /// </summary>
        /// <param name="transmitterStep">����������ö��</param>
        /// <param name="step">����������</param>
        void Regist(TransmitterSteps transmitterStep, ITransmitteStep step);
        /// <summary>
        ///     ִ����һ����������
        /// </summary>
        void Next(params object[] args);
        /// <summary>
        ///     ��ǰ���ȵķ����¼�
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<string>> Processing;
    }
}