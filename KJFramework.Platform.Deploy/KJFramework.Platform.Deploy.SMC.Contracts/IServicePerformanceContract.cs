using KJFramework.Platform.Deploy.SMC.Common.Performances;
using KJFramework.ServiceModel.Core.Attributes;

namespace KJFramework.Platform.Deploy.SMC.Contracts
{
    /// <summary>
    ///     �������ܻ㱨����ԼԪ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    [ServiceContract(Description = "�������ܻ㱨����Լ", Name = "Service Performance Contract", Version = "0.0.0.1")]
    public interface IServicePerformanceContract
    {
        /// <summary>
        ///     ��ȡ������Ϣ
        /// </summary>
        /// <param name="serviceName">��������</param>
        /// <param name="performanceFlag">
        ///     ���ܱ�ʶ
        /// <para>* �����ܱ�ʶ�������</para>
        /// </param>
        /// <returns>���ط���������Ϣ</returns>
        ServicePerformanceItem[] GetPerformance(string serviceName, string performanceFlag);
        /// <summary>
        ///     ��ȡ�ܿط�Χ�����з����������Ϣ
        /// </summary>
        /// <param name="performanceFlag">
        ///     ���ܱ�ʶ
        /// <para>* �����ܱ�ʶ�������</para>
        /// </param>
        /// <returns>���ط���������Ϣ</returns>
        ServicePerformanceItem[] GetPerformance(string performanceFlag);
    }
}