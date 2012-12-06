using System;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Platform.Deploy.Metadata.Objects
{
    /// <summary>
    ///     ���ķ�����Ϣ����
    /// </summary>
    public class CoreServiceItem : IntellectObject
    {
        #region Members

        /// <summary>
        ///     ��ȡ�����÷�����Ϣ
        /// </summary>
        [IntellectProperty(0, IsRequire = true)]
        public string Category { get; set; }
        /// <summary>
        ///     ��ȡ�����û�����
        /// </summary>
        [IntellectProperty(1, IsRequire = true)]
        public string MachineName { get; set; }
        /// <summary>
        ///     ��ȡ�����ÿ��Ƶ�ַ
        /// </summary>
        [IntellectProperty(2, IsRequire = false)]
        public string ControlAddress { get; set; }
        /// <summary>
        ///     ��ȡ�������������ʱ��
        /// </summary>
        [IntellectProperty(3, IsRequire = false)]
        public DateTime LastHeartBeatTime { get; set; }
        /// <summary>
        ///     ��ȡ�����ò����ַ
        /// </summary>
        [IntellectProperty(4, IsRequire = false)]
        public string DeployAddress { get; set; }
        /// <summary>
        ///     ��ȡ�������ڲ��ķ�����Ϣ
        /// </summary>
        [IntellectProperty(5, IsRequire = false)]
        public OwnServiceItem[] Services { get; set; }

        #endregion
    }
}