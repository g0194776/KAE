using System;

namespace KJFramework.Diagnostics
{
    /// <summary>
    ///     ��Ϣ������Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IInfomationReviewer
    {
        /// <summary>
        ///     ע����Ϣ�ռ���
        /// </summary>
        /// <param name="collector">��Ϣ�ռ���</param>
        void Regist(IInfomationCollector collector);
        /// <summary>
        ///     ע����Ϣ�ռ���
        /// </summary>
        /// <param name="collector">��Ϣ�ռ���</param>
        void UnRegist(IInfomationCollector collector);
        /// <summary>
        ///     ע����Ϣ�ռ���
        /// </summary>
        /// <param name="id">��Ϣ�ռ���Ψһ��ʾ</param>
        void UnRegist(Guid id);
        /// <summary>
        ///     ��ȡ����ָ��Ψһ��ʾ����Ϣ�ռ���
        /// </summary>
        /// <param name="id">��Ϣ�ռ���Ψһ��ʾ</param>
        /// <returns>���ض�Ӧ����Ϣ�ռ���</returns>
        IInfomationCollector GetCollector(Guid id);
    }
}