using System;
using System.Collections.Generic;
using System.Text;

namespace KJFramework.ApplicationEngine.Resources
{
    /// <summary>
    ///    KAE请求资源URI
    ///    <para>* 这个资源URI将会被用于计算灰度的应用等级</para>
    /// </summary>
    public class KAEResourceUri : Dictionary<string, string>
    {
        #region Members.

        private static readonly string _uriHeader = "http://www.kae.com/resource?";

        #endregion

        #region Methods.

        /// <summary>
        ///    从一个HTTP URI中解析出一个KAE资源URI
        /// </summary>
        /// <param name="uri">HTTP URI</param>
        /// <returns>返回解析后的KAE资源URI</returns>
        public static KAEResourceUri Parse(string uri)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(_uriHeader);
            foreach (KeyValuePair<string, string> pair in this)
                builder.Append(pair.Key).Append('=').Append(pair.Value).Append('&');
            return builder.ToString();
        }

        #endregion
    }
}