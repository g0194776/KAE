using System;
namespace KJFramework.Security
{
    /// <summary>
    ///     ֤��Ԫ�ӿڣ��ṩ����صĻ�����ȫ���ԡ�
    /// </summary>
    public interface ICertificate
    {
        /// <summary>
        ///     ��ȡ������֤�����
        /// </summary>
        String Category { get; set; }
        /// <summary>
        ///     ��ȡ������֤����Ȩ��
        /// </summary>
        String Certigier { get; set; }
        /// <summary>
        ///     ��ȡ������֤���ʼ��ʱ��
        /// </summary>
        DateTime InitializeTime { get; set; }
        /// <summary>
        ///     ��ȡ������֤�����ʱ��
        ///         * �������ʱ��Ϊnull, ���ʾ��ǰ֤���������ڡ�
        /// </summary>
        DateTime? ExpiredTime { get; set; }
        /// <summary>
        ///     ��ȡ������һ��ֵ����ֵָʾ�˵�ǰ֤���Ƿ��Ѿ�������
        /// </summary>
        bool IsExpired { get; set; }
    }
}