using KJFramework.Platform.Deploy.CSN.CP.Connector.Enums;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Subscribers;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Transmitters.Contexts;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector.Transmitters.Steps
{
    /// <summary>
    ///     ���䲽��Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface ITransmitteStep
    {
        /// <summary>
        ///     ִ��һ������
        /// </summary>
        /// <param name="configSubscriber">���ö�����</param>
        /// <param name="context">������</param>
        /// <param name="args">��ز���</param>
        /// <returns>����ִ�к��״̬</returns>
        TransmitterSteps Do(IConfigSubscriber configSubscriber, ITransmitterContext context, params object[] args);
    }
}