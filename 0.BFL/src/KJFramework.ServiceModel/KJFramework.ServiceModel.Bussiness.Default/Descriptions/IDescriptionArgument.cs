using System;

namespace KJFramework.ServiceModel.Bussiness.Default.Descriptions
{
    /// <summary>
    ///     ���������ӿڣ��ṩ����صĻ������Խṹ
    /// </summary>
    public interface IDescriptionArgument : IDisposable
    {
        /// <summary>
        ///     ��ȡ�����ò���˳����
        /// </summary>
        int Id { get; set; }
        /// <summary>
        ///     ��ȡ�����ò���ȫ����
        /// </summary>
        String FullName { get; set; }
        /// <summary>
        ///     ��ȡ�����ò�������
        /// </summary>
        String Name { get; set; }
        /// <summary>
        ///     ��ȡ������һ����ʾ����ʾ��ǰ�����Ƿ����Ϊ��
        /// </summary>
        bool CanNull { get; set; }
        /// <summary>
        ///     ��ȡ�����ò�������
        /// </summary>
        Type ParameterType { get; set; }
    }
}