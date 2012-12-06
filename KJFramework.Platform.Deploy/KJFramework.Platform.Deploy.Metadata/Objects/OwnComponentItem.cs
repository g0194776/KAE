using System;
using KJFramework.Basic.Enum;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Platform.Deploy.Metadata.Objects
{
    /// <summary>
    ///     ӵ�������
    /// </summary>
    public class OwnComponentItem : IntellectObject
    {
        /// <summary>
        ///     ��ȡ�������������
        /// </summary>
        [IntellectProperty(0, IsRequire = false)]
        public string Name { get; set;}
        /// <summary>
        ///     ��ȡ�������������
        /// </summary>
        [IntellectProperty(1, IsRequire = true)]
        public string ComponentName { get; set; }
        /// <summary>
        ///     ��ȡ����������汾��
        /// </summary>
        [IntellectProperty(2, IsRequire = false)]
        public string Version { get; set; }
        /// <summary>
        ///     ��ȡ�����÷�������
        /// </summary>
        [IntellectProperty(3, IsRequire = false)]
        public string Description { get; set; }
        /// <summary>
        ///     ��ȡ�������������״̬
        /// </summary>
        [IntellectProperty(4, IsRequire = true)]
        public HealthStatus Status { get; set; }
        /// <summary>
        ///     ��ȡ������������ʱ��
        /// </summary>
        [IntellectProperty(5, IsRequire = false)]
        public DateTime LastUpdateTime { get; set; }
    }
}