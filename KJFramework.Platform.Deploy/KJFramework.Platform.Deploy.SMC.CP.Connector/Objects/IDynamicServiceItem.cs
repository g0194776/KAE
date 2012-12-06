using System;
using KJFramework.Platform.Deploy.Metadata.Enums;
using KJFramework.Platform.Deploy.Metadata.Objects;
using KJFramework.Platform.Deploy.Metadata.Performances;
using KJFramework.Platform.Deploy.SMC.Common.Performances;

namespace KJFramework.Platform.Deploy.SMC.CP.Connector.Objects
{
    /// <summary>
    ///     ��̬�����¼��Ԫ�ӿ�
    /// </summary>
    public interface IDynamicServiceItem
    {
        /// <summary>
        ///     ��ȡͨ�����
        /// </summary>
        Guid ChannelId { get; } 
        /// <summary>
        ///     ��ȡ��������
        /// </summary>
        string ServiceName { get; }
        /// <summary>
        ///     ��ȡ����汾��
        /// </summary>
        string Version { get; }
        /// <summary>
        ///     ��ȡ��������
        /// </summary>
        string Description { get; }
        /// <summary>
        ///     ��ȡ�������
        /// </summary>
        string Name { get; }
        /// <summary>
        ///     ��ȡ��������ǰ汾��
        /// </summary>
        string ShellVersion { get; set; }
        /// <summary>
        ///     ��ȡ������һ��ֵ����ֵ��ʾ�˵�ǰ��̬�����Ƿ�֧�ֳ���������ܲ���
        /// </summary>
        bool SupportDomainPerformance { get; set; }
        /// <summary>
        ///     ��ȡ�����÷�����״̬
        /// </summary>
        ServiceLiveStatus LiveStatus { get; set; }
        /// <summary>
        ///     ��ȡ�����÷����Ӧ�ó��������
        /// </summary>
        int AppDomainCount { get; set; }
        /// <summary>
        ///     ��ȡ�����÷�����������
        /// </summary>
        int ComponentCount { get; set; }
        /// <summary>
        ///     ��ȡ�������������ʱ��
        /// </summary>
        DateTime LastHeartBeatTime { get; set; }
        /// <summary>
        ///     ��ȡ������������ʱ��
        /// </summary>
        DateTime LastUpdateTime { get; set; }
        /// <summary>
        ///     ��ȡ��������������Ϣ
        /// </summary>
        String LastError { get; set; }
        /// <summary>
        ///     ��ȡ�����ý�������
        /// </summary>
        string ProcessName { get; set; }
        /// <summary>
        ///     ��ȡ�������������ϸ��Ϣ
        /// </summary>
        ComponentDetailItem[] Components { get; set; }
        /// <summary>
        ///     ����������
        /// </summary>
        /// <param name="items">������</param>
        void Update(ServicePerformanceItem[] items);
        /// <summary>
        ///     ����������
        /// </summary>
        /// <param name="items">������</param>
        void Update(DomainPerformanceItem[] items);
        /// <summary>
        ///     ����������
        /// </summary>
        /// <param name="items">������</param>
        void Update(ComponentHealthItem[] items);
        /// <summary>
        ///     ����������½����
        /// </summary>
        /// <param name="items">���½����</param>
        void Update(ComponentUpdateResultItem[] items);
        /// <summary>
        ///     ��ȡ�������������
        /// </summary>
        /// <returns>���ط������������</returns>
        ServicePerformanceItem[] GetPerformances();
        /// <summary>
        ///     ��ȡ����Ӧ�ó��������������
        /// </summary>
        /// <returns>���ط������������</returns>
        DomainPerformanceItem[] GetDomainPerformances();
    }
}