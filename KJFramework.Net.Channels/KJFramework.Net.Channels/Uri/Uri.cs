using KJFramework.Tracing;
using System;

namespace KJFramework.Net.Channels.Uri
{
    /// <summary>
    ///     资源地址标示类，提供了相关的基本操作。
    /// </summary>
    public abstract class Uri
    {
        #region 成员

        protected String _prefix;
        protected String _address;
        protected String _url;
        protected String _splitFlag = "://";
        protected string _serverUri;
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof (Uri));

        /// <summary>
        ///     获取或设置完整的URL
        /// </summary>
        public String Url
        {
            get { return _url; }
            set
            {
                _url = value;
                Split();
            }
        }

        /// <summary>
        ///     获取或设置地址路径
        /// </summary>
        public String Address
        {
            get { return _address; }
            set { _address = value; }
        }

        /// <summary>
        ///     获取或设置前缀
        /// </summary>
        public String Prefix
        {
            get { return _prefix; }
            set { _prefix = value; }
        }

        #endregion

        #region 构造函数

        /// <summary>
        ///     资源地址标示类，提供了相关的基本操作。
        /// </summary>
        public Uri() : this("")
        { }

        /// <summary>
        ///     资源地址标示类，提供了相关的基本操作。
        /// </summary>
        /// <param name="url" type="string">
        ///     <para>
        ///         完整的URL地址
        ///     </para>
        /// </param>
        public Uri(String url)
        {
            Url = url;
        }

        #endregion

        #region 父类方法

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return _url;
        }

        /// <summary>
        ///     获取服务器内部使用的Uri形态
        /// </summary>
        /// <returns>返回Uri</returns>
        public abstract string GetServiceUri();

        #endregion

        #region 方法

        protected virtual void Split()
        {
            if (_url == null)
            {
                throw new System.Exception("资源地址标示不能为空。");
            }
            if (_url.Trim() == "")
            {
                return;
            }
            try
            {
                int flagStartOffset;
                if ((flagStartOffset = _url.IndexOf(_splitFlag)) == -1)
                {
                    throw new System.Exception("非法的资源地址标示。");
                }
                String prefix = _url.Substring(0, flagStartOffset);
                String address = _url.Substring(flagStartOffset + _splitFlag.Length, _url.Length - (flagStartOffset + _splitFlag.Length));
                if (String.IsNullOrEmpty(prefix) || String.IsNullOrEmpty(address))
                {
                    throw new System.Exception("非法的资源地址标示。");
                }
                _prefix = prefix;
                _address = address;
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                throw new System.Exception("非法的资源地址标示。");
            }
        }

        #endregion
    }
}