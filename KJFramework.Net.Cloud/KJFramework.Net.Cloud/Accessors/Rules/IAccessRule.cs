using System;

namespace KJFramework.Net.Cloud.Accessors.Rules
{
    /// <summary>
    ///     ���ʹ���Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IAccessRule : IDisposable
    {
        /// <summary>
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰ�����Ƿ�����
        /// </summary>
        bool Accessed { get; }
    }
}