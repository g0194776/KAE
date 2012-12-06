using KJFramework.Platform.Deploy.CSN.CP.Connector.Objects;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Subscribers;
using KJFramework.Platform.Deploy.CSN.ProtocolStack;
using KJFramework.Platform.Deploy.CSN.ProtocolStack.Enums;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector.Transmitters.Contexts
{
    /// <summary>
    ///     ������������Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface ITransmitterContext
    {
        /// <summary>
        ///     ��ȡ������������
        /// </summary>
        int TaskId { get; set; }
        /// <summary>
        ///     ��ȡ�������ܹ������ݳ���
        /// </summary>
        int TotalDataLength { get; set; }
        /// <summary>
        ///     ��ȡ�������ܹ��ķְ�����
        /// </summary>
        int TotalPackageCount { get; set; }
        /// <summary>
        ///     ��ȡ��������һ��������Ϣ�ĻỰ���
        /// </summary>
        int PreviousSessionId { get; set; }
        /// <summary>
        ///     ��ȡ�����÷ְ����ݼ���
        /// </summary>
        DataPart[] Datas { get; set; }
        /// <summary>
        ///     ��ȡ���������ö�����
        /// </summary>
        IConfigSubscriber Subscriber { get; set; }
        /// <summary>
        ///     ��ȡ�����û�����Ϣ
        /// </summary>
        CSNMessage ResponseMessage { get; set; }
        /// <summary>
        ///     ��ȡ��������������
        /// </summary>
        ConfigTypes ConfigType { get; set; }
        /// <summary>
        ///     ��ȡ����ָ���ؼ�ֵ��ֵ
        /// </summary>
        /// <typeparam name="T">����ֵ������</typeparam>
        /// <param name="key">�ؼ���</param>
        /// <returns>����ֵ</returns>
        T Get<T>(string key);
        /// <summary>
        ///     ���һ��ֵ
        /// </summary>
        /// <param name="key">�ؼ���</param>
        /// <param name="value">Ҫ��ӵĶ���</param>
        void Add(string key, object value);
        /// <summary>
        ///     �Ƴ�һ������ָ���ؼ��ֵ�ֵ
        /// </summary>
        /// <param name="key">�ؼ���</param>
        void Remove(string key);
    }
}