using System;
using KJFramework.Enums;

namespace KJFramework.Tasks
{
    /// <summary>
    ///     ����Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface ITask : IDisposable
    {
        /// <summary>
        ///     ��ȡ��������������
        /// </summary>
        String Description { get; set; }
        /// <summary>
        ///     ��ȡ����������Ψһ��ʾ
        /// </summary>
        int Id { get; set; }
        /// <summary>
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰ�����Ƿ��Ѿ����
        /// </summary>
        bool IsFinished { get; }
        /// <summary>
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰ�����Ƿ��Ѿ�ȡ��
        /// </summary>
        bool IsCanceled { get; }
        /// <summary>
        ///     ��ȡ���񴴽�ʱ��
        /// </summary>
        DateTime CreateTime { get; }
        /// <summary>
        ///     ��ȡ�������������ʱ��
        ///             * �������Ϊnull, ���ʾ��Զ������
        /// </summary>
        DateTime? ExpiredTime { get; set; }
        /// <summary>
        ///     ��ȡ�������������ȼ�
        /// </summary>
        TaskPriority Priority { get; set; }
        /// <summary>
        ///     ȡ������
        /// </summary>
        void Cancel();
        /// <summary>
        ///     ִ������
        /// </summary>
        void Execute();
        /// <summary>
        ///     �첽ִ������
        /// </summary>
        void ExecuteAsyn();
        event EventHandler ExecuteSuccessful , ExecuteFail;
    }
}