using System;
using System.Net;

namespace KJFramework.Net.Cloud.Urls.Parsers
{
    /// <summary>
    ///   TCP协议的服务域资源地址解析器，提供了相关的基本操作。
    /// </summary>
    public class ServerAreaTcpUriParser : ServerAreaUriParser
    {
        #region 构造函数

        /// <summary>
        ///   TCP协议的服务域资源地址解析器，提供了相关的基本操作。
        /// </summary>
        public ServerAreaTcpUriParser()
        {
            _protocolKey = "tcp";
        }


        #endregion

        #region Overrides of ServerAreaUriParser

        /// <summary>
        ///   从给定的资源中解析出一个服务域连接地址对象
        /// </summary>
        /// <param name="uri">除了资源地址头以外的部分</param>
        /// <param name="keys">关键字结合</param>
        /// <returns>服务域连接地址对象</returns>
        public override IServerAreaUri Parse(string uri, string[] keys)
        {
            if (String.IsNullOrEmpty(uri) || keys == null || keys.Length != 2)
            {
                throw new System.Exception("非法的解析条件。");
            }
            String[] resources = uri.Split(new[] {":"}, StringSplitOptions.RemoveEmptyEntries);
            if (resources.Length != 2)
            {
                throw new System.Exception("非法的资源地址。");
            }
            IPEndPoint iep = new IPEndPoint(IPAddress.Parse(resources[0]), int.Parse(resources[1]));
            return new TcpServerAreaUri(iep);
        }

        #endregion
    }
}