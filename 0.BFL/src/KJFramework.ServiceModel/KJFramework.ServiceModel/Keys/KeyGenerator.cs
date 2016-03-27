using System;
using System.Threading;
using KJFramework.Encrypt;

namespace KJFramework.ServiceModel.Keys
{
    /// <summary>
    ///     Ψһ��ʾ���������ṩ�˴���صĻ���������
    /// </summary>
    public class KeyGenerator
    {
        #region ��Ա

        private static long _count;
        private static int _currentKey;

        #endregion

        /// <summary>
        ///     ʹ��ʱ����Ϊ����������һ��Ψһ��ʾ
        /// </summary>
        /// <returns>����Ψһ��ʾ</returns>
        public static int Generate()
        {
            return Interlocked.Increment(ref _currentKey);
        }

        /// <summary>
        ///     ʹ�ñ�ʾ��Ϊ����������һ��Ψһ��ʾ
        /// </summary>
        /// <param name="key">��ʾ</param>
        /// <returns>����Ψһ��ʾ</returns>
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
        ///     ʹ�÷���ȫ���Լ���������������һ��Ψһ��ʾ
        /// </summary>
        /// <param name="methodName">����ȫ��</param>
        /// <param name="argsCount">��������</param>
        /// <returns>����Ψһ��ʾ</returns>
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