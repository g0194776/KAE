using System;
using KJFramework.Basic.Enum;
using KJFramework.Logger.LogObject;

namespace KJFramework.Logger
{
    /// <summary>
    ///     ���ּ�¼��Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITextLogger<T> : ILogger<T>, IDisposable
        where T : ITextLog
    {
        /// <summary>
        ///     ��ָ����¼������ӵ���¼������
        ///           *  ��¼�����Զ��ж��Ƿ�Ϊͷ����ʾ��
        ///           *  ʹ�ô˷�������¼���쳣���ȵȼ�Ĭ��Ϊ����ͨ
        /// </summary>
        /// <param name="exception">�쳣����</param>
        void Log(System.Exception exception);
        /// <summary>
        ///     ��ָ����¼������ӵ���¼������
        ///           *  ��¼�����Զ��ж��Ƿ�Ϊͷ����ʾ��
        /// </summary>
        /// <param name="exception">�쳣����</param>
        /// <param name="grade">�쳣�ȼ�</param>
        void Log(System.Exception exception, DebugGrade grade);
        /// <summary>
        ///     ��ָ����¼������ӵ���¼������
        ///           *  ��¼�����Զ��ж��Ƿ�Ϊͷ����ʾ��
        /// </summary>
        /// <param name="exception">�쳣����</param>
        /// <param name="grade">�쳣�ȼ�</param>
        /// <param name="name">
        ///     ��¼��
        ///         * [ע] �� ���������ͷ���ã���������Ϊnull
        /// </param>
        void Log(System.Exception exception, DebugGrade grade, String name);
        /// <summary>
        ///     ��ָ����¼������ӵ���¼������
        /// </summary>
        /// <param name="exception">�쳣����</param>
        /// <param name="grade">�쳣�ȼ�</param>
        /// <param name="isHead">ͷ��ʾ</param>
        /// <param name="name">
        ///     ��¼��
        ///         * [ע] �� ���������ͷ���ã���������Ϊnull
        /// </param>
        void Log(System.Exception exception, DebugGrade grade, bool isHead, String name);
        /// <summary>
        ///     ��ָ���������ݼ�¼����¼������
        /// </summary>
        /// <param name="text">��������</param>
        void Log(String text);
    }
}