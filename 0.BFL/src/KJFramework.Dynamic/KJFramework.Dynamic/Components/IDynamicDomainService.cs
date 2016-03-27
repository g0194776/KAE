using System;
using KJFramework.Dynamic.Structs;
using KJFramework.Enums;
using KJFramework.EventArgs;

namespace KJFramework.Dynamic.Components
{
    /// <summary>
    ///     ��̬���������Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IDynamicDomainService
    {
        /// <summary>
        ///     ��ȡ�ڲ��������
        /// </summary>
        int ComponentCount { get; }
        /// <summary>
        ///     ��ȡ�����ø�������
        /// </summary>
        Object Tag { get; set; }
        /// <summary>
        ///     ��ȡΨһ��ʾ
        /// </summary>
        Guid Id { get; }
        /// <summary>
        ///     ��ȡ����Ŀ¼
        /// </summary>
        String WorkRoot { get; }
        /// <summary>
        ///     ��ȡ����������Ϣ
        /// </summary>
        ServiceDescriptionInfo Infomation { get; }
        /// <summary>
        ///     ��ʼִ��
        /// </summary>
        void Start();
        /// <summary>
        ///     ִֹͣ��
        /// </summary>
        void Stop();
        /// <summary>
        ///     ���·���
        /// </summary>
        /// <returns>���ظ��µ�״̬</returns>
        bool Update();
        /// <summary>
        ///     ���¾���ָ��ȫ�������
        /// </summary>
        /// <param name="fullname">���ȫ��</param>
        /// <returns>���ظ��µ�״̬</returns>
        /// <exception cref="ArgumentNullException">��������Ϊ��</exception>
        bool Update(string fullname);
        /// <summary>
        ///     ��齡��״��
        /// </summary>
        /// <returns>���ؽ���״��</returns>
        HealthStatus CheckHealth();
        /// <summary>
        ///     ����������ƻ�ȡһ�����������
        /// </summary>
        /// <param name="name">����</param>
        /// <returns>���ػ�ȡ���ĳ��������</returns>
        IDynamicDomainComponent GetObject(String name);
        /// <summary>
        ///     ��ʼ����
        /// </summary>
        event EventHandler StartWork;
        /// <summary>
        ///     ֹͣ����
        /// </summary>
        event EventHandler EndWork;
        /// <summary>
        ///     ����״̬�㱨�¼�
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<String>> WorkingProcess;
        /// <summary>
        ///     ����״̬�㱨�¼�
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<String>> Updating;
    }
}