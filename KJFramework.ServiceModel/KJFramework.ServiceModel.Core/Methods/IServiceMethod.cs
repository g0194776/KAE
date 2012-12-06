using System;
using KJFramework.ServiceModel.Core.Attributes;

namespace KJFramework.ServiceModel.Core.Methods
{
    /// <summary>
    ///     ���񷽷�Ԫ�ӿڣ��ṩ����صĻ�������
    /// </summary>
    public interface IServiceMethod : IDisposable
    {
        /// <summary>
        ///     ��ȡ�����ò�������
        /// </summary>
        OperationAttribute Attribute { get; set; }
        /// <summary>
        ///     ��ȡ������Ψһ���
        /// </summary>
        Guid Id { get;  }
        /// <summary>
        ///     ��ȡ�����ò�������
        /// </summary>
        int ArgsCount { get; set; }
        /// <summary>
        ///     ��ȡ�����÷�����
        /// </summary>
        String Name { get; set; }
        /// <summary>
        ///     ��ȡ������һ��ֵ����ֵ��ʾ�˵�ǰ�����Ƿ���з���ֵ
        ///     <para>* void ����ӵ�з���ֵ��</para>
        /// </summary>
        bool HasReturnValue { get; set; }
        /// <summary>
        ///     ��ȡ�����÷���ֵ����
        /// </summary>
        Type ReturnType { get; set; }
        /// <summary>
        ///     ��ȡ��������
        /// </summary>
        /// <param name="paraIndex">��������</param>
        /// <returns>���ز�������</returns>
        Type GetParameterType(int paraIndex);
    }
}