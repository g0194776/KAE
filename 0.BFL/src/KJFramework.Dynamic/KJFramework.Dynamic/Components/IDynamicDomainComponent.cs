using System;
using KJFramework.Enums;

namespace KJFramework.Dynamic.Components
{
    /// <summary>
    ///     ��̬���������Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IDynamicDomainComponent
    {
        /// <summary>
        ///     ��ȡ����
        /// </summary>
        String Name { get; }
        /// <summary>
        ///     ��鵱ǰ����Ľ���״��
        /// </summary>
        /// <returns>���ؽ���״��</returns>
        HealthStatus CheckHealth();
        /// <summary>
        ///     ��ȡΨһ��ʾ
        /// </summary>
        Guid Id { get; }
        /// <summary>
        ///      ��ȡ�����ÿ��ñ�ʾ
        /// </summary>
        bool Enable { get; set; }
        /// <summary>
        ///     ��ȡ�����ò����Ϣ
        /// </summary>
        PluginInfomation PluginInfo { get; }
        /// <summary>
        ///     ��ȡ�����õ�ǰ����������ķ���
        /// </summary>
        IDynamicDomainService OwnService { get; set; }
        /// <summary>
        ///     ��ʼִ��
        /// </summary>
        void Start();
        /// <summary>
        ///     ִֹͣ��
        /// </summary>
        void Stop();
        /// <summary>
        ///     ���غ���Ҫ���Ķ���
        /// </summary>
        void OnLoading();
    }
}