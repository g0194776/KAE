using System;
using System.Threading;
using KJFramework.Encrypt;

namespace KJFramework.ServiceModel.Keys
{
    /// <summary>
    ///     唯一标示生成器，提供了从相关的基本操作。
    /// </summary>
    public class KeyGenerator
    {
        #region 成员

        private static long _count;
        private static int _currentKey;

        #endregion

        /// <summary>
        ///     使用时间作为参数来生成一串唯一标示
        /// </summary>
        /// <returns>返回唯一标示</returns>
        public static int Generate()
        {
            return Interlocked.Increment(ref _currentKey);
        }

        /// <summary>
        ///     使用标示作为参数来生成一串唯一标示
        /// </summary>
        /// <param name="key">标示</param>
        /// <returns>返回唯一标示</returns>
        public static String Generate(String key)
        {
            lock (typeof(KeyGenerator))
            {
                if (_count > 900000000000000)
                {
                    _count = 0;
                }
                String result = EncryptHashHelper.HashString(DateTime.Now.Ticks + key + ++_count);
                return result;
            }
        }

        /// <summary>
        ///     使用方法全名以及参数个数来生成一串唯一标示
        /// </summary>
        /// <param name="methodName">方法全名</param>
        /// <param name="argsCount">参数个数</param>
        /// <returns>返回唯一标示</returns>
        public static String Generate(String methodName, int argsCount)
        {
            lock (typeof(KeyGenerator))
            {
                if (_count > 900000000000000)
                {
                    _count = 0;
                }
                return EncryptHashHelper.HashString(String.Format("{0}:{1}:{2}:{3}", DateTime.Now.Millisecond, methodName, argsCount, ++_count));
            }
        }
    }
}