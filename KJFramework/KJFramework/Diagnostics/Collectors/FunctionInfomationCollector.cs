using System;
using KJFramework.Enums;

namespace KJFramework.Diagnostics.Collectors
{
    /// <summary>
    ///     ������Ϣ�ռ��������࣬�ṩ����صĻ���������
    /// </summary>
    public abstract class FunctionInfomationCollector : InfomationCollector, IFunctionInfomationCollector
    {
        #region ���캯��

        /// <summary>
        ///     ������Ϣ�ռ��������࣬�ṩ����صĻ���������
        /// </summary>
        /// <param name="collectType">�ռ���������</param>
        /// <param name="reviewere">��Ϣ������</param>
        protected FunctionInfomationCollector(Type collectType, IInfomationReviewer reviewere)
            : base(collectType, reviewere)
        {
            _infomationCollectorType = InfomationCollectorTypes.Special;
        }

        #endregion

        #region Implementation of IFunctionInfomationCollector

        /// <summary>
        ///     ֪ͨ
        /// </summary>
        /// <typeparam name="T">�������</typeparam>
        /// <param name="args">����</param>
        /// <returns>����֪ͨ�Ľ��</returns>
        public abstract T Notify<T>(params object[] args);

        #endregion
    }
}