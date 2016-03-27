using System;
using System.Collections.Generic;

namespace KJFramework.Net.Cloud.Urls.Parsers
{
    /// <summary>
    ///   服务域资源地址解析器集合，提供了相关的基本操作。
    /// </summary>
    public class ServeAreaUriParsers
    {
        #region 构造函数

        /// <summary>
        ///   服务域资源地址解析器集合，提供了相关的基本操作。
        /// </summary>
        private ServeAreaUriParsers()
        {
            Regist(new ServerAreaTcpUriParser());
        }

        #endregion

        #region Members

        private Dictionary<String, IServerAreaUriParser> _parsers = new Dictionary<string, IServerAreaUriParser>();
        /// <summary>
        ///   服务域资源地址解析器集合，提供了相关的基本操作。
        /// </summary>
        public static readonly ServeAreaUriParsers Instance = new ServeAreaUriParsers();

        #endregion

        #region Functions

        /// <summary>
        ///   注册解析器
        ///   <para>* 如果存在指定的ProtocolKey解析器，则进行替换。</para>
        /// </summary>
        /// <param name="parser">解析器</param>
        public void Regist(IServerAreaUriParser parser)
        {
            if (parser == null || String.IsNullOrEmpty(parser.ProtocolKey))
            {
                throw new System.Exception("非法的解析器");
            }
            if (_parsers.ContainsKey(parser.ProtocolKey))
            {
                _parsers[parser.ProtocolKey] = parser;
                return;
            }
            _parsers.Add(parser.ProtocolKey, parser);
        }

        /// <summary>
        ///   通过一个协议关键字来获取对应的资源地址解析器
        /// </summary>
        /// <param name="protocolKey">协议关键字</param>
        /// <returns>返回资源地址解析器</returns>
        public IServerAreaUriParser GetParser(String protocolKey)
        {
            if (String.IsNullOrEmpty(protocolKey))
            {
                return null;
            }
            return _parsers.ContainsKey(protocolKey) ? _parsers[protocolKey] : null;
        }

        #endregion
    }
}