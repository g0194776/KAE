using System;
using KJFramework.Basic.Enum;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Platform.Deploy.Metadata.Objects
{
    /// <summary>
    ///     �����ϸ��Ϣ�����
    /// </summary>
    public class ComponentDetailItem : IntellectObject
    {
        #region Members

        /// <summary>
        ///     ��ȡ�������������
        /// </summary>
        [IntellectProperty(0, IsRequire = true)]
        public string Name { get; set; }
        /// <summary>
        ///     ��ȡ�����ý���״̬
        /// </summary>
        [IntellectProperty(1, IsRequire = false)]
        public HealthStatus Status { get; set; }
        /// <summary>
        ///     ��ȡ����������汾
        /// </summary>
        [IntellectProperty(2, IsRequire = false)]
        public string Version { get; set; }
        /// <summary>
        ///     ��ȡ���������������Ϣ
        /// </summary>
        [IntellectProperty(3, IsRequire = false)]
        public string Description { get; set; }
        /// <summary>
        ///     ��ȡ�����������ȫ��
        /// </summary>
        [IntellectProperty(4, IsRequire = false)]
        public string ServiceName { get; set; }
        /// <summary>
        ///     ��ȡ�������齨�ķ���
        /// </summary>
        [IntellectProperty(5, IsRequire = false)]
        public string CatalogName { get; set; }
        /// <summary>
        ///     ��ȡ���������������ʱ��
        /// </summary>
        [IntellectProperty(6, IsRequire = false)]
        public DateTime LastUpdateTime { get; set; }

        #endregion

    }
}