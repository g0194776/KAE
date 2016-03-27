using System;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Platform.Deploy.Metadata.Objects
{
    /// <summary>
    ///     �ļ���ϸ��Ϣ����
    /// </summary>
    public class FileInfo : IntellectObject
    {
        /// <summary>
        ///     ��ȡ�������ļ�Ŀ¼�ṹ
        /// </summary>
        [IntellectProperty(0, IsRequire = false)]
        public string Directory { get; set; }
        /// <summary>
        ///     ��ȡ�������ļ�����
        /// </summary>
        [IntellectProperty(1, IsRequire = true)]
        public string FileName { get; set; }
        /// <summary>
        ///     ��ȡ�������ļ�����޸�ʱ��
        /// </summary>
        [IntellectProperty(2, IsRequire = false)]
        public DateTime LastModifyTime { get; set; }
        /// <summary>
        ///     ��ȡ�������ļ�����ʱ��
        /// </summary>
        [IntellectProperty(3, IsRequire = false)]
        public DateTime CreateTime { get; set; }
        /// <summary>
        ///     ��ȡ�������ļ���С
        /// </summary>
        [IntellectProperty(4, IsRequire = true)]
        public double Size { get; set; }
        /// <summary>
        ///     ��ȡ������һ��ֵ����ֵ��ʾ�˵�ǰ�ļ��Ƿ����
        /// </summary>
        [IntellectProperty(5, IsRequire = true)]
        public bool IsExists { get; set; }
    }
}