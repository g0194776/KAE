using System;
using KJFramework.Net.Enums;
using KJFramework.Tracing;

namespace KJFramework.Net.Uri
{
    /// <summary>
    ///     ×ÊÔ´µØÖ·±êÊ¾Àà£¬Ìá¹©ÁËÏà¹ØµÄ»ù±¾²Ù×÷¡£
    /// </summary>
    public abstract class Uri : MarshalByRefObject
    {
        #region ³ÉÔ±

        protected String _prefix;
        protected String _address;
        protected String _url;
        protected String _splitFlag = "://";
        protected string _serverUri;
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof (Uri));

        /// <summary>
        ///     »ñÈ¡»òÉèÖÃÍêÕûµÄURL
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
        ///    获取当前URL所代表的网络类型
        /// </summary>
        public abstract NetworkTypes NetworkType { get; }

        /// <summary>
        ///     »ñÈ¡»òÉèÖÃµØÖ·Â·¾¶
        /// </summary>
        public String Address
        {
            get { return _address; }
            set { _address = value; }
        }

        /// <summary>
        ///     »ñÈ¡»òÉèÖÃÇ°×º
        /// </summary>
        public String Prefix
        {
            get { return _prefix; }
            set { _prefix = value; }
        }

        #endregion

        #region ¹¹Ôìº¯Êý

        /// <summary>
        ///     ×ÊÔ´µØÖ·±êÊ¾Àà£¬Ìá¹©ÁËÏà¹ØµÄ»ù±¾²Ù×÷¡£
        /// </summary>
        public Uri() : this("")
        { }

        /// <summary>
        ///     ×ÊÔ´µØÖ·±êÊ¾Àà£¬Ìá¹©ÁËÏà¹ØµÄ»ù±¾²Ù×÷¡£
        /// </summary>
        /// <param name="url" type="string">
        ///     <para>
        ///         ÍêÕûµÄURLµØÖ·
        ///     </para>
        /// </param>
        public Uri(String url)
        {
            Url = url;
        }

        #endregion

        #region ¸¸Àà·½·¨

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
        ///     »ñÈ¡·þÎñÆ÷ÄÚ²¿Ê¹ÓÃµÄUriÐÎÌ¬
        /// </summary>
        /// <returns>·µ»ØUri</returns>
        public abstract string GetServiceUri();

        #endregion

        #region ·½·¨

        protected virtual void Split()
        {
            if (_url == null)
            {
                throw new System.Exception("×ÊÔ´µØÖ·±êÊ¾²»ÄÜÎª¿Õ¡£");
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
                    throw new System.Exception("·Ç·¨µÄ×ÊÔ´µØÖ·±êÊ¾¡£");
                }
                String prefix = _url.Substring(0, flagStartOffset);
                String address = _url.Substring(flagStartOffset + _splitFlag.Length, _url.Length - (flagStartOffset + _splitFlag.Length));
                if (String.IsNullOrEmpty(prefix) || String.IsNullOrEmpty(address))
                {
                    throw new System.Exception("·Ç·¨µÄ×ÊÔ´µØÖ·±êÊ¾¡£");
                }
                _prefix = prefix;
                _address = address;
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                throw new System.Exception("·Ç·¨µÄ×ÊÔ´µØÖ·±êÊ¾¡£");
            }
        }

        #endregion
    }
}