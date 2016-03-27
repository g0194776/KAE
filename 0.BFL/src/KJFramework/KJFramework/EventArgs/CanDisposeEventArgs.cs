using System;
namespace KJFramework.EventArgs
{
    /// <summary>
    ///     ֧��ע�����¼�
    /// </summary>
    public class CanDisposeEventArgs : System.EventArgs, IDisposable
    {
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