using System;

namespace KJFramework.ServiceModel.Core.Methods
{
    /// <summary>
    ///     ��ִ�еķ��񷽷�Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    internal interface IExecutableServiceMethod : IServiceMethod
    {
        /// <summary>
        ///     ��ȡ����������ʵ��
        /// </summary>
        Object Instance { get; set; }
        /// <summary>
        ///     ִ�е�ǰ���񷽷�
        /// </summary>
        /// <param name="args">��������</param>
        /// <returns>
        ///     ���ص�ǰ���񷽷��ķ���ֵ
        ///     <para>* �����ǰ���������з���ֵ����Ҳ�᷵��null��</para>
        /// </returns>
        Object Invoke(params Object[] args);
    }
}