using System.Threading;

namespace KJFramework.Platform.Deploy.CSN.Common
{
    /// <summary>
    ///     �Ự���������
    /// </summary>
    public static class SessionGenerator
    {
        #region Members

        private static int _current = 0;

        #endregion

        #region Methods

        /// <summary>
        ///     ���Դ���һ���µĻỰ���
        /// </summary>
        /// <returns>�����µĻỰ���</returns>
        public static int Create()
        {
            Interlocked.CompareExchange(ref _current, 0, int.MaxValue);
            return Interlocked.Increment(ref _current);
        }

        #endregion
    }
}