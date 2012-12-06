using System;
using KJFramework.Basic.Enum;
using KJFramework.Dynamic.Exceptions;
using KJFramework.Dynamic.Structs;
using KJFramework.Plugin;

namespace KJFramework.Dynamic.Components
{
    /// <summary>
    ///     ��̬����������ṩ����صĻ���������
    /// </summary>
    internal interface IDynamicDomainObject : IDisposable
    {
        /// <summary>
        ///     ��ȡ�ڲ���̬���������
        /// </summary>
        IDynamicDomainComponent Component { get; }
        /// <summary>
        ///     ��鵱ǰ����Ľ���״��
        /// </summary>
        /// <returns>���ؽ���״��</returns>
        HealthStatus CheckHealth();
        /// <summary>
        ///     ��ȡ����ʱ��
        /// </summary>
        DateTime CreateTime { get; }
        /// <summary>
        ///     ��ȡ�������ϴθ���ʱ��
        /// </summary>
        DateTime LastUpdateTime { get; set; }
        /// <summary>
        ///     ��ȡӦ�ó������齨�����Ϣ
        /// </summary>
        DomainComponentEntryInfo EntryInfo { get; }
        /// <summary>
        ///     ��ȡ�������ϸ��Ϣ
        /// </summary>
        PluginInfomation Infomation { get; }
        /// <summary>
        ///     ��ȡ�ڲ�Ӧ�ó�����
        /// </summary>
        /// <returns>����Ӧ�ó�����</returns>
        AppDomain GetDomain();
        /// <summary>
        ///     ��ȡΨһ��ʾ
        /// </summary>
        Guid Id { get; }
        /// <summary>
        ///     ��ʼִ��
        /// </summary>
        void Start();
        /// <summary>
        ///     ִֹͣ��
        /// </summary>
        void Stop();
        /// <summary>
        ///     ���µ�ǰ��̬������
        /// </summary>
        /// <exception cref="DynamicDomainObjectUpdateFailedException">����ʧ��</exception>
        void Update();
        /// <summary>
        ///     ���������������������
        /// </summary>
        /// <param name="time">����ʱ��</param>
        void ReLease(TimeSpan time);
        /// <summary>
        ///     ��̬����������˳��¼�
        /// </summary>
        event EventHandler Exited;
    }
}