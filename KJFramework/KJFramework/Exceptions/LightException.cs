using System;

namespace KJFramework.Exceptions
{
    /// <summary>
    ///     实现了IDisposable接口的轻量级异常类。
    /// </summary>
    public class LightException : System.Exception, IDisposable
    {
        #region 构造函数

        /// <summary>
        ///     实现了IDisposable接口的轻量级异常类。
        /// </summary>
        public LightException()
        {
            
        }

        /// <summary>
        ///     实现了IDisposable接口的轻量级异常类。
        /// </summary>
        /// <param name="message">消息内容</param>
        public LightException(String message)
            : base(message)
        {

        }

        #endregion

        #region 析构函数

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