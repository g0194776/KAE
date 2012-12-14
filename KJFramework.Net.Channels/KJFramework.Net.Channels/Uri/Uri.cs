using KJFramework.Tracing;
using System;

namespace KJFramework.Net.Channels.Uri
{
    /// <summary>
    ///     ��Դ��ַ��ʾ�࣬�ṩ����صĻ���������
    /// </summary>
    public abstract class Uri
    {
        #region ��Ա

        protected String _prefix;
        protected String _address;
        protected String _url;
        protected String _splitFlag = "://";
        protected string _serverUri;
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof (Uri));

        /// <summary>
        ///     ��ȡ������������URL
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
        ///     ��ȡ�����õ�ַ·��
        /// </summary>
        public String Address
        {
            get { return _address; }
            set { _address = value; }
        }

        /// <summary>
        ///     ��ȡ������ǰ׺
        /// </summary>
        public String Prefix
        {
            get { return _prefix; }
            set { _prefix = value; }
        }

        #endregion

        #region ���캯��

        /// <summary>
        ///     ��Դ��ַ��ʾ�࣬�ṩ����صĻ���������
        /// </summary>
        public Uri() : this("")
        { }

        /// <summary>
        ///     ��Դ��ַ��ʾ�࣬�ṩ����صĻ���������
        /// </summary>
        /// <param name="url" type="string">
        ///     <para>
        ///         ������URL��ַ
        ///     </para>
        /// </param>
        public Uri(String url)
        {
            Url = url;
        }

        #endregion

        #region ���෽��

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
        ///     ��ȡ�������ڲ�ʹ�õ�Uri��̬
        /// </summary>
        /// <returns>����Uri</returns>
        public abstract string GetServiceUri();

        #endregion

        #region ����

        protected virtual void Split()
        {
            if (_url == null)
            {
                throw new System.Exception("��Դ��ַ��ʾ����Ϊ�ա�");
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
                    throw new System.Exception("�Ƿ�����Դ��ַ��ʾ��");
                }
                String prefix = _url.Substring(0, flagStartOffset);
                String address = _url.Substring(flagStartOffset + _splitFlag.Length, _url.Length - (flagStartOffset + _splitFlag.Length));
                if (String.IsNullOrEmpty(prefix) || String.IsNullOrEmpty(address))
                {
                    throw new System.Exception("�Ƿ�����Դ��ַ��ʾ��");
                }
                _prefix = prefix;
                _address = address;
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                throw new System.Exception("�Ƿ�����Դ��ַ��ʾ��");
            }
        }

        #endregion
    }
}