using System.Net;

namespace KJFramework.Net.Extends
{
    public static class StringExtends
    {
        #region Methods.

        /// <summary>
        ///    将一个指定格式的字符串解析为一个IPEndPoint对象
        ///     <para>格式为: xxxxxx:xxx</para>
        /// </summary>
        /// <param name="content">指定格式内容的字符串</param>
        /// <returns>返回解析后的IPEndPoint对象</returns>
        public static IPEndPoint ConvertToIPEndPoint(this string content)
        {
            //create new channel by connection str.
            int splitOffset = content.LastIndexOf(':');
            string ip = content.Substring(0, splitOffset);
            int port = int.Parse(content.Substring(splitOffset + 1, content.Length - (splitOffset + 1)));
            return new IPEndPoint(IPAddress.Parse(ip), port);
        }


        #endregion
    }
}