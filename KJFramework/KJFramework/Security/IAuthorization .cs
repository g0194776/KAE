using System;

namespace KJFramework.Security
{
    /// <summary>
    ///     ��ȨԪ�ӿڣ��ṩ����صĻ������Խṹ
    /// </summary>
    public interface IAuthorization
    {
        /// <summary>
        ///     ��ȡ��Ȩ��
        /// </summary>
        String Certigier { get; }
        /// <summary>
        ///     ��ȡ��Ȩ���
        /// </summary>
        String Category { get; }
        /// <summary>
        ///     ��ȡ��Ȩ����
        /// </summary>
        DateTime Time { get; }
    }
}