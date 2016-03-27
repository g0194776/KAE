using System;

namespace KJFramework.Messages.Attributes
{
    /// <summary>
    ///     ��������Ԫ�ӿڣ��ṩ����صĻ������Խṹ��
    /// </summary>
    public interface IIntellectProperty
    {
        /// <summary>
        ///     ��ȡ����˳����
        ///     <para>* �˱�Ų����ظ���</para>
        /// </summary>
        int Id { get; }
        /// <summary>
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰ�����Ƿ����ӵ��ֵ��
        /// </summary>
        bool IsRequire { get; }
        /// <summary>
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰ�����Ƿ���Ҫ������չ���춯����
        ///     <para>* ����Ӱ�췶Χ����������Ϣ�ṹ��������</para>
        /// </summary>
        bool NeedExtendAction { get; }
        /// <summary>
        ///     ��ȡ��������
        ///     <para>* ����Ӱ�췶Χ����������Ϣ�ṹ��������</para>
        /// </summary>
        String Tag { get; }
    }
}