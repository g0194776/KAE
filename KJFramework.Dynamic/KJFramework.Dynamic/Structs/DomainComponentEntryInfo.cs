using System;

namespace KJFramework.Dynamic.Structs
{
    /// <summary>
    ///     �����������Ϣ�ṹ��
    /// </summary>
    public class DomainComponentEntryInfo
    {
        /// <summary>
        ///     ��ȡ�������ļ���ַ
        /// </summary>
        public String FilePath { get; set; }
        /// <summary>
        ///     ��ȡ�������ļ��е�ַ
        /// </summary>
        public String FolderPath { get; set; }
        /// <summary>
        ///     ��ȡ��������ڵ��ַ
        /// </summary>
        public String EntryPoint { get; set; }
        /// <summary>
        ///     ��ȡ�����ð汾��Ϣ
        /// </summary>
        String Version { get; set; }
    }
}