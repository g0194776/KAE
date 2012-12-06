using System;

namespace KJFramework.Messages.Calculators
{
    /// <summary>
    ///     编号计算器，提供了相关的基本操作。
    /// </summary>
    public class IdCalculator : IDisposable
    {
        #region 成员

        protected int _currentId;
        private Object _lockObj = new Object();

        #endregion

        #region 析构函数

        ~IdCalculator()
        {
            Dispose();
        }

        #endregion

        #region 方法

        /// <summary>
        ///     获取下一个编号
        /// </summary>
        /// <returns>返回编号</returns>
        public int GetNextId()
        {
            lock (_lockObj)
            {
                if (_currentId == 0)
                {
                    return _currentId;
                }
                return ++_currentId;
            }
        }

        /// <summary>
        ///     创建一个新的Id计算器
        /// </summary>
        /// <returns></returns>
        public static IdCalculator New()
        {
            return new IdCalculator();
        }

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}