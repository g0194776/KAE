using System;
using KJFramework.Basic.Enum;
using KJFramework.EventArgs;

namespace KJFramework.Diagnostics
{
    /// <summary>
    ///     ��Ϣ�ռ���Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IInfomationCollector : ICollector
    {
        /// <summary>
        ///     �ռ�ʱ����
        ///            * ʱ�䵥λ�� ���롣
        /// </summary>
        double CollectInterval { get; set; }
        /// <summary>
        ///     ��ȡ�����ñ��ռ���Ϣ�Ķ�������
        /// </summary>
        Type CollectType { get; }
        /// <summary>
        ///     ��ȡ����ʱ��
        /// </summary>
        DateTime CreateTime { get; }
        /// <summary>
        ///     ��ȡ�����ø�������
        /// </summary>
        Object Tag { get; set; }
        /// <summary>
        ///     ��ȡ��Ϣ�ռ�������ö��
        /// </summary>
        InfomationCollectorTypes InfomationCollectorType { get; }
        /// <summary>
        ///     ��ȡ������
        /// </summary>
        /// <returns>����������</returns>
        IInfomationReviewer GetReviewer();
        /// <summary>
        ///     �����ռ�ʱ��
        /// </summary>
        /// <param name="interval">�ռ�ʱ��</param>
        void ResetCollectInterval(double interval);
        /// <summary>
        ///     ����Ϣ�¼�
        /// </summary>
        event EventHandler<NewInfomationEventArgs> NewInfomation;
        /// <summary>
        ///     �ռ�ʱ�䱻�����¼�
        /// </summary>
        event EventHandler IntervalTimeChanged;
    }
}