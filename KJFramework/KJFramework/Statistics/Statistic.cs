using System;
using KJFramework.Enums;

namespace KJFramework.Statistics
{
    /// <summary>
    ///     统计器抽象类，提供了相关的基本操作。
    /// </summary>
    public abstract class Statistic : IStatistic
    {
        #region 构造函数

        /// <summary>
        ///     统计器抽象类，提供了相关的基本操作。
        /// </summary>
        public Statistic()
        {
            _isEnable = true;
        }

        #endregion

        #region 析构函数

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
        ///     获取统计类型
        /// </summary>
        public StatisticTypes StatisticType
        {
            get { return _statisticType; }
        }

        /// <summary>
        ///     获取或设置可用标示
        /// </summary>
        public bool IsEnable
        {
            get { return _isEnable; }
            set { _isEnable = value; }
        }

        /// <summary>
        ///     初始化
        /// </summary>
        /// <param name="element">统计类型</param>
        /// <typeparam name="T">统计类型</typeparam>
        public abstract void Initialize<T>(T element);

        /// <summary>
        ///     关闭统计
        /// </summary>
        public abstract void Close();

        #endregion
    }
}