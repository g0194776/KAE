using System;

namespace KJFramework.Messages.Calculators
{
    /// <summary>
    ///     ��ż��������ṩ����صĻ���������
    /// </summary>
    public class IdCalculator : IDisposable
    {
        #region ��Ա

        protected int _currentId;
        private Object _lockObj = new Object();

        #endregion

        #region ��������

        ~IdCalculator()
        {
            Dispose();
        }

        #endregion

        #region ����

        /// <summary>
        ///     ��ȡ��һ�����
        /// </summary>
        /// <returns>���ر��</returns>
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
        ///     ����һ���µ�Id������
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