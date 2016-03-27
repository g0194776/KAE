using System;

namespace KJFramework
{
    /// <summary>
    ///     ��������������ԼԪ�ӿڣ��ṩ����صĻ�������
    /// </summary>
    public interface ICacheLease
    {
        /// <summary>
        ///     ��ȡ������һ��ֵ����ֵ��ʾ�˵�ǰ�Ļ����Ƿ�֧�ֳ�ʱ���
        ///     <para>* ���CanTimeout = false, ��ExpireTime = max(DateTime)</para>
        /// </summary>
        bool CanTimeout { get; set; }
        /// <summary>
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰ�Ļ����Ƿ��Ѿ�����������״̬
        /// </summary>
        bool IsDead { get; }
        /// <summary>
        ///     ��ȡ�������ڴ�����ʱ��
        /// </summary>
        DateTime CreateTime { get; }
        /// <summary>
        ///     ��ȡ��ʱʱ��
        /// </summary>
        DateTime ExpireTime { get; }
        /// <summary>
        ///     ����ǰ���������������Ϊ����״̬
        /// </summary>
        void Discard();
        /// <summary>
        ///     ����ǰ������Լһ��ʱ��
        /// </summary>
        /// <param name="timeSpan">��Լʱ��</param>
        /// <returns>������Լ��ĵ���ʱ��</returns>
        /// <exception cref="System.Exception">����ʧ��</exception>
        DateTime Renew(TimeSpan timeSpan);
    }
}