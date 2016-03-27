using System.Security.Cryptography;
using System.Text;

namespace KJFramework.Data.ObjectDB.Helpers
{
    internal static class UtilityHelper
    {
        #region Members

        private static readonly MD5 _md5Provider = new MD5CryptoServiceProvider();

        #endregion

        #region Methods

        /// <summary>
        ///     计算一个字符串的唯一编号
        /// </summary>
        /// <param name="fullname">字符串</param>
        /// <returns>返回唯一编号</returns>
        public unsafe static ulong CalcTokenId(string fullname)
        {
            byte[] hashData = _md5Provider.ComputeHash(Encoding.UTF8.GetBytes(fullname));
            fixed (byte* pByte = hashData) return *(ulong*)(pByte + 8);
        }

        #endregion
    }
}