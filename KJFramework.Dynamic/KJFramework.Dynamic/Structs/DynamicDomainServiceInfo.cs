using System;
using KJFramework.Dynamic.Components;

namespace KJFramework.Dynamic.Structs
{
    /// <summary>
    ///     ��̬�����������Ϣ����
    /// </summary>
    public struct DynamicDomainServiceInfo
    {
        /// <summary>
        ///     ��ȡ�����ö�̬���������
        /// </summary>
        public IDynamicDomainService Service { get; set; }
        /// <summary>
        ///     ��ȡ�������ļ���ַ
        /// </summary>
        public String FilePath { get; set; }
        /// <summary>
        ///     ��ȡ������Ŀ¼��Ϣ
        /// </summary>
        public String DirectoryPath { get; set; }
    }
}