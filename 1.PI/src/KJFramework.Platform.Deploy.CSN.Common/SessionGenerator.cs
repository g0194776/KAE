using System.Threading;

namespace KJFramework.Platform.Deploy.CSN.Common
{
    /// <summary>
    ///     会话编号生成器
    /// </summary>
    public static class SessionGenerator
    {
        #region Members

        private static int _current = 0;

        #endregion

        #region Methods

        /// <summary>
        ///     尝试创建一个新的会话编号
        /// </summary>
        /// <returns>返回新的会话编号</returns>
        public static int Create()
        {
            Interlocked.CompareExchange(ref _current, 0, int.MaxValue);
            return Interlocked.Increment(ref _current);
        }

        #endregion
    }
}