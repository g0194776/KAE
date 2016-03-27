using System;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;
using KJFramework.Platform.Deploy.Metadata.Enums;
using KJFramework.Platform.Deploy.SMC.Common.Performances;

namespace KJFramework.Platform.Deploy.Metadata.Objects
{
    /// <summary>
    ///     ӵ�з�����
    /// </summary>
    public class OwnServiceItem : IntellectObject
    {
        /// <summary>
        ///     ��ȡ�����÷������
        /// </summary>
        [IntellectProperty(0, IsRequire = false)]
        public string Name { get; set; }
        /// <summary>
        ///     ��ȡ�����÷�������
        /// </summary>
        [IntellectProperty(1, IsRequire = true)]
        public string ServiceName { get; set; }
        /// <summary>
        ///     ��ȡ�����÷���汾��
        /// </summary>
        [IntellectProperty(2, IsRequire = false)]
        public string Version { get; set; }
        /// <summary>
        ///     ��ȡ�����÷�������
        /// </summary>
        [IntellectProperty(3, IsRequire = false)]
        public string Description { get; set; }
        /// <summary>
        ///     ��ȡ�������������
        /// </summary>
        [IntellectProperty(4, IsRequire = true)]
        public int ComponentCount { get; set; }
        /// <summary>
        ///     ��ȡ�������������
        /// </summary>
        [IntellectProperty(5, IsRequire = false)]
        public OwnComponentItem[] Componnets { get; set; }
        /// <summary>
        ///     ��ȡ������������ʱ��
        /// </summary>
        [IntellectProperty(6, IsRequire = false)]
        public DateTime LastUpdateTime { get; set; }
        /// <summary>
        ///     ��ȡ�������������ʱ��
        /// </summary>
        [IntellectProperty(7, IsRequire = false)]
        public DateTime LastHeartbeatTime { get; set; }
        /// <summary>
        ///     ��ȡ�����ÿ��Ʒ����ַ
        /// </summary>
        [IntellectProperty(8, IsRequire = false)]
        public string ControlServiceAddress { get; set; }
        /// <summary>
        ///     ��ȡ�����÷�����״̬
        /// </summary>
        [IntellectProperty(9, IsRequire = false)]
        public ServiceLiveStatus LiveStatus { get; set; }
        /// <summary>
        ///     ��ȡ�������Ƿ�֧�ֳ���������ܼ��
        /// </summary>
        [IntellectProperty(10, IsRequire = false)]
        public bool SupportDomainPerformance { get; set; }
        /// <summary>
        ///     ��ȡ��������ǰ汾��
        /// </summary>
        [IntellectProperty(11, IsRequire = false)]
        public string ShellVersion { get; set; }
        /// <summary>
        ///     ��ȡ�����÷����е�����
        /// </summary>
        [IntellectProperty(12, IsRequire = false)]
        public ServicePerformanceItem[] PerformanceItems { get; set; }
        /// <summary>
        ///     ��ȡ�����÷���Ӧ�ó����������
        /// </summary>
        [IntellectProperty(13, IsRequire = false)]
        public DomainPerformanceItem[] DomainPerformanceItems { get; set; }
    }
}