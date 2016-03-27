using System;

namespace KJFramework.ServiceModel.Bussiness.Default.Descriptions
{
    /// <summary>
    ///     ��Լ��ϢԪ�ӿڣ��ṩ����ص����Խṹ
    /// </summary>
    public interface IContractInfomation
    {
        /// <summary>
        ///     ��ȡ��������Լ����ʱ��
        /// </summary>
        DateTime OpenTime { get; set; }
        /// <summary>
        ///     ��ȡ��������Լ����
        /// </summary>
        String ContractName { get; set; }
        /// <summary>
        ///     ��ȡ��������Լ����
        /// </summary>
        String Name { get; set; }
        /// <summary>
        ///     ��ȡ��������Լȫ����
        /// </summary>
        string FullName { get; set; }
        /// <summary>
        ///     ��ȡ��������Լ����
        /// </summary>
        String Description { get; set; }
        /// <summary>
        ///     ��ȡ��������Լ�汾
        /// </summary>
        String Version { get; set; }
    }
}