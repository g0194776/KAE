using System;
using KJFramework.ServiceModel.Core.Attributes;
using KJFramework.ServiceModel.Core.Objects;

namespace KJFramework.ServiceModel.Bussiness.Default.Services
{
    /// <summary>
    ///     ��������Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    internal interface IHostService
    {
        /// <summary>
        ///     ��ȡ�ڲ���������
        /// </summary>
        /// <returns>���ط�������</returns>
        Type GetServiceType();
        /// <summary>
        ///     ��ȡԪ������Լ����
        /// </summary>
        /// <returns>������Լ���ŵ�����</returns>
        string GetContractName();
        /// <summary>
        ///     ����һ���µ��ڲ�����ʵ��
        /// </summary>
        /// <returns>�����µ��ڲ�����ʵ��</returns>
        object NewServiceInstance();
        /// <summary>
        ///     �黹һ���ڲ�����ʵ��
        /// </summary>
        /// <param name="obj">�ڲ�����ʵ��</param>
        void Giveback(object obj);
        /// <summary>
        ///     ��ȡ���з��񷽷�
        /// </summary>
        /// <returns></returns>
        ServiceMethodPickupObject[] GetMethods();
        /// <summary>
        ///     ��ȡ����ָ�����ƺͲ��������ķ��񷽷�
        /// </summary>
        /// <param name="methodToken">�������б��</param>
        /// <returns>���ط���</returns>
        ServiceMethodPickupObject GetMethod(int methodToken);
        /// <summary>
        ///     ��ȡ�������
        /// </summary>
        /// <returns></returns>
        Object GetServiceObject();
        /// <summary>
        ///     ��ȡΨһ���
        /// </summary>
        Guid Id { get; }
        /// <summary>
        ///     ��ȡ����ʱ��
        /// </summary>
        DateTime CreateTime { get; }
        /// <summary>
        ///     ��ȡ������Լ����
        /// </summary>
        ServiceContractAttribute Contract { get; }
    }
}