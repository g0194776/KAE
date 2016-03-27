using System;

namespace KJFramework.Security
{
    /// <summary>
    ///     令牌，提供了相关的基本安全属性。
    /// </summary>
    [Serializable]
    public class Token : IToken
    {
        #region 构造函数

        /// <summary>
        ///     令牌，提供了相关的基本安全属性。
        /// </summary>
        /// <param name="content">令牌内容</param>
        public Token(byte[] content)
        {
            _content = content;
        }

        #endregion

        #region IToken Members

        private byte[] _content;

        /// <summary>
        ///     获取令牌内容
        /// </summary>
        public byte[] Content
        {
            get { return _content; }
        }

        #endregion
    }
}