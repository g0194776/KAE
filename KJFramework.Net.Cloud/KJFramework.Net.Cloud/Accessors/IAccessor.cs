using System;
using KJFramework.Net.Cloud.Accessors.Rules;

namespace KJFramework.Net.Cloud.Accessors
{
    /// <summary>
    ///     ������Ԫ�ӿڣ��ṩ����صĽ񱾲�����
    /// </summary>
    public interface IAccessor : IDisposable
    {
        /// <summary>
        ///     ��ȡһ��������ʹ���
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="taget">����</param>
        /// <returns>���ط��ʹ���</returns>
        IAccessRule GetAccessRule<T>(T taget);
    }
}