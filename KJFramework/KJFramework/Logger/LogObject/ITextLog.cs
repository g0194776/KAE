using System;

namespace KJFramework.Logger.LogObject
{
    /// <summary>
    ///     ��־�ı���¼��Դ�ӿڣ��ṩ����ص����Խṹ��
    /// </summary>
    public interface ITextLog : IDebugLog
    {
        /// <summary>
        ///     ��ȡ��ǰҪд����־�����ݡ�
        ///         * [ע] �������ֱ�ӷ��ش��и�ʽ����־���ݡ�
        /// </summary>
        /// <returns>������־������</returns>
        String GetLogContent();
        /// <summary>
        ///     ��ȡ��־ͷ����Ϣ
        /// </summary>
        /// <returns>����ͷ����Ϣ</returns>
        String GetHead();
        /// <summary>
        ///     ��ȡ�����õ�ǰ��¼���Ƿ���ͷ����¼�
        /// </summary>
        bool IsHead { get; set; }
        /// <summary>
        ///     ��ȡ��������־��¼���ʽ
        /// </summary>
        ITextLogFormatter Formatter { get; set; }
    }
}