using System;

namespace KJFramework
{
    /// <summary>
    ///     ֻ���Ļ�������������ԼԪ�ӿڣ��ṩ����صĻ�������
    /// </summary>
    public interface IReadonlyCacheLease
    {
        /// <summary>
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰ�Ļ����Ƿ�֧�ֳ�ʱ���
        /// </summary>
        bool CanTimeout { get; }
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
    }
}