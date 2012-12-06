using KJFramework.Platform.Deploy.Metadata.Enums;
using KJFramework.ServiceModel.Core.Attributes;

namespace KJFramework.Platform.Deploy.SMC.Contracts
{
    /// <summary>
    ///     �����������ԼԪ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    [ServiceContract(Description = "�����������Լ", Name = "Service Controller Contract", Version = "0.0.0.1")]
    public interface IServiceControllerContract
    {
        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="serviceName">��������</param>
        /// <returns>���ط���״̬</returns>
        [Operation]
        ServiceStatus Open(string serviceName);
        /// <summary>
        ///     �رշ���
        /// </summary>
        /// <param name="serviceName">��������</param>
        /// <returns>���ط���״̬</returns>
        [Operation]
        ServiceStatus Close(string serviceName);
        /// <summary>
        ///     ��ͣ����
        /// </summary>
        /// <param name="serviceName">��������</param>
        /// <returns>���ط���״̬</returns>
        [Operation]
        ServiceStatus Pause(string serviceName);
        /// <summary>
        ///     ��ѯ����״̬
        /// </summary>
        /// <param name="serviceName">��������</param>
        /// <returns>���ط���״̬</returns>
        [Operation]
        ServiceStatus GetStatus(string serviceName);
    }
}