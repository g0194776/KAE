using System.ServiceProcess;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Platform.Deploy.Metadata.Objects
{
    /// <summary>
    ///     ������ϸ��Ϣ����
    /// </summary>
    public class ServiceInfoItem : IntellectObject
    {
        #region Members

        /// <summary>
        ///     ��ȡ�����÷�������
        /// </summary>
        [IntellectProperty(0, IsRequire = true)]
        public string ServiceName { get; set; }
        /// <summary>
        ///     ��ȡ�����÷������
        /// </summary>
        [IntellectProperty(1, IsRequire = false)]
        public string Name { get; set; }
        /// <summary>
        ///     ��ȡ�����÷�������
        /// </summary>
        [IntellectProperty(2, IsRequire = false)]
        public string Description { get; set; }
        /// <summary>
        ///     ��ȡ�����÷���״̬
        /// </summary>
        [IntellectProperty(3, IsRequire = true)]
        public ServiceControllerStatus Status { get; set; }

        #endregion
    }
}