using System;
namespace KJFramework.Logger.LogObject
{
    /// <summary>
    ///     ������־���ʽԪ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface ITextLogFormatter
    {
        /// <summary>
        ///    ��ȡ�Ϸָ��
        /// </summary>
        String Up { get; }
        /// <summary>
        ///    ��ȡ�Ϸָ��
        /// </summary>
        String Down { get; }
        /// <summary>
        ///     ��ȡ���ָ���
        /// </summary>
        String LeftSplitChar { get; }
    }
}