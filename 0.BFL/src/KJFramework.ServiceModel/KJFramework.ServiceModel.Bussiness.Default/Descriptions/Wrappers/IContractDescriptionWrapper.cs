using System;

namespace KJFramework.ServiceModel.Bussiness.Default.Descriptions.Wrappers
{
    /// <summary>
    ///     ��Լ������װ��Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IContractDescriptionWrapper
    {
        /// <summary>
        ///     ��ȡ��Լ����
        /// </summary>
        IContractDescription ContractDescription { get; }
        /// <summary>
        ///     ��ȡ��Լ�������ı���ʽ
        /// </summary>
        /// <returns>������Լ�������ı���ʽ</returns>
        String GetContent();
    }
}