using System;

namespace KJFramework.Exceptions
{
    /// <summary>
    ///     ʵ����IDisposable�ӿڵ��������쳣�ࡣ
    /// </summary>
    public class LightException : System.Exception, IDisposable
    {
        #region ���캯��

        /// <summary>
        ///     ʵ����IDisposable�ӿڵ��������쳣�ࡣ
        /// </summary>
        public LightException()
        {
            
        }

        /// <summary>
        ///     ʵ����IDisposable�ӿڵ��������쳣�ࡣ
        /// </summary>
        /// <param name="message">��Ϣ����</param>
        public LightException(String message)
            : base(message)
        {

        }

        #endregion

        #region ��������

        ~LightException()
        {
            Dispose();
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