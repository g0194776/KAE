using System;
namespace KJFramework.Diagnostics
{
    /// <summary>
    ///     ������Ϣ�ռ���Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IFunctionInfomationCollector : IInfomationCollector
    {
        /// <summary>
        ///     ֪ͨ
        /// </summary>
        /// <typeparam name="T">�������</typeparam>
        /// <param name="args">����</param>
        /// <returns>����֪ͨ�Ľ��</returns>
        T Notify<T>(params Object[] args);
    }
}