using System;

namespace KJFramework.ServiceModel.Contexts
{
    /// <summary>
    ///     ������Լ�������������ģ��ṩ����صĻ������Խṹ��
    /// </summary>
    public interface IServiceCallContext
    {
        /// <summary>
        ///     ��ȡ������Ψһ���
        /// </summary>
        int Id { get; set; }
        /// <summary>
        ///     ��ȡ�Ự��Կ
        /// </summary>
        int SessionId { get; }
        /// <summary>
        ///     ��ȡ����ʵ��
        /// </summary>
        Object Instance { get; }
    }
}