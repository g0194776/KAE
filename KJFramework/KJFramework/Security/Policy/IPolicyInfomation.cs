using System;
namespace KJFramework.Security.Policy
{
    /// <summary>
    ///     ������Ϣ�ӿڣ��ṩ����صĻ������ԡ�
    /// </summary>
    public interface IPolicyInfomation
    {
        /// <summary>
        ///     ��ȡ����������
        /// </summary>
        String Name { get; set; }
        /// <summary>
        ///     ��ȡ�����԰汾
        /// </summary>
        String Version { get; set; }
        /// <summary>
        ///     ��ȡ������������Ϣ
        /// </summary>
        String Description { get; set; }
    }
}