using System;
using KJFramework.Results;

namespace KJFramework.Diagnostics
{
    /// <summary>
    ///     �ռ���Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface ICollector : IControlable, IDisposable
    {
        /// <summary>
        ///     ��ȡΨһ��ʾ
        /// </summary>
        Guid Id { get; }
        /// <summary>
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰ�ռ����Ƿ����
        /// </summary>
        bool IsActive { get; }
        event EventHandler BeginWork, EndWork;
    }
}