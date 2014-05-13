using System;
using KJFramework.Enums;

namespace KJFramework.Statistics
{
    /// <summary>
    ///     ͳ���������࣬�ṩ����صĻ���������
    /// </summary>
    public abstract class Statistic : IStatistic
    {
        #region ���캯��

        /// <summary>
        ///     ͳ���������࣬�ṩ����صĻ���������
        /// </summary>
        public Statistic()
        {
            _isEnable = true;
        }

        #endregion

        #region ��������

        ~Statistic()
        {
            Dispose();
        }

        #endregion

        #region Implementation of IDisposable

        protected StatisticTypes _statisticType;
        protected bool _isEnable;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Implementation of IStatistic

        /// <summary>
        ///     ��ȡͳ������
        /// </summary>
        public StatisticTypes StatisticType
        {
            get { return _statisticType; }
        }

        /// <summary>
        ///     ��ȡ�����ÿ��ñ�ʾ
        /// </summary>
        public bool IsEnable
        {
            get { return _isEnable; }
            set { _isEnable = value; }
        }

        /// <summary>
        ///     ��ʼ��
        /// </summary>
        /// <param name="element">ͳ������</param>
        /// <typeparam name="T">ͳ������</typeparam>
        public abstract void Initialize<T>(T element);

        /// <summary>
        ///     �ر�ͳ��
        /// </summary>
        public abstract void Close();

        #endregion
    }
}