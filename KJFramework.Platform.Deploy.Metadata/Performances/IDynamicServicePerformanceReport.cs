using System;
using KJFramework.Platform.Deploy.Metadata.Enums;
using KJFramework.Platform.Deploy.SMC.Common.Performances;

namespace KJFramework.Platform.Deploy.Metadata.Performances
{
    /// <summary>
    ///     ��̬�������ܱ���Ԫ�ӿڣ��ṩ����صĻ�������
    /// </summary>
    public interface IDynamicServicePerformanceReport
    {
        /// <summary>
        ///     ��ȡ�����÷�������
        /// </summary>
        string ServiceName { get; set; }
        /// <summary>
        ///     ��ȡ�����÷���汾
        /// </summary>
        string Version { get; set; }
        /// <summary>
        ///     ��ȡ�����÷�������
        /// </summary>
        string Description { get; set; }
        /// <summary>
        ///     ��ȡ�����÷������
        /// </summary>
        string Name { get; set; }
        /// <summary>
        ///     ��ȡ������Ӧ�ó��������
        /// </summary>
        int AppDomainCount { get; set; }
        /// <summary>
        ///     ��ȡ�������������
        /// </summary>
        int ComponentCount { get; set; }
        /// <summary>
        ///     ��ȡ������������ʱ��
        /// </summary>
        DateTime LastHeartBeatTime { get; set; }
        /// <summary>
        ///     ��ȡ������һ��ֵ����ֵ��ʾ�˵�ǰ��̬�����Ƿ�֧�ֳ���������ܲ���
        /// </summary>
        bool SupportDomainPerformance { get; set; }
        /// <summary>
        ///     ��ȡ��������������Ϣ
        /// </summary>
        String LastError { get; set; }
        /// <summary>
        ///     ��ȡ������״̬
        /// </summary>
        ServiceLiveStatus LiveStatus { get; set; }
        /// <summary>
        ///     ��ȡ������������
        /// </summary>
        ServicePerformanceItem[] PerformanceItems { get; set; }
        /// <summary>
        ///     ��ȡ������Ӧ�ó�����������
        /// </summary>
        DomainPerformanceItem[] DomainItems { get; set; }
        /// <summary>
        ///     ��ȡ����������Ľ���״̬
        /// </summary>
        ComponentHealthItem[] ComponentItems { get; set; }
    }
}