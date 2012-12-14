using KJFramework.Tracing;
using System;

namespace KJFramework.Net.Channels.Uri
{
    /// <summary>
    ///     Tcp��Դ��ַ��ʾ�࣬�ṩ����صĻ���������
    /// </summary>
    public class TcpUri : Uri
    {
        #region ��Ա

        protected String _hostAddress;
        protected String _serviceName;
        private int _port;
        protected bool _isHost;
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof (TcpUri));

        /// <summary>
        ///     ��ȡ�����÷�������
        /// </summary>
        public String ServiceName
        {
            get { return _serviceName; }
            set { _serviceName = value; }
        }

        /// <summary>
        ///     ��ȡ������������ַ
        /// </summary>
        public String HostAddress
        {
            get { return _hostAddress; }
            set { _hostAddress = value; }
        }

        /// <summary>
        ///     ��ȡ�����������˿�
        /// </summary>
        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }

        /// <summary>
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰ������������ַ�Ƿ�Ϊ���ص�ַ��
        /// </summary>
        public bool IsHost
        {
            get { return _isHost; }
        }

        #endregion

        #region ���캯��

        /// <summary>
        ///     Tcp��Դ��ַ��ʾ�࣬�ṩ����صĻ���������
        /// </summary>
        public TcpUri() : base("")
        { }

        /// <summary>
        ///     Tcp��Դ��ַ��ʾ�࣬�ṩ����صĻ���������
        /// </summary>
        /// <param name="url" type="string">
        ///     <para>
        ///         ������URL��ַ
        ///     </para>
        /// </param>
        public TcpUri(String url)
            : base(url)
        {
            int offset = url.LastIndexOf(':');
            _serverUri = url.Substring(offset, url.Length - offset);
        }

        #endregion

        #region ���෽��

        /// <summary>
        ///     ��ȡ�������ڲ�ʹ�õ�Uri��̬
        /// </summary>
        /// <returns>����Uri</returns>
        public override string GetServiceUri()
        {
            return _serverUri;
        }

        protected override void Split()
        {
            base.Split();
            try
            {
                if (_prefix.ToLower() != "tcp")
                {
                    throw new System.Exception("�Ƿ���TCP��Դ��ַ��ʾ��");
                }
                int firstFlagOffset = _address.IndexOf("/");
                if (firstFlagOffset == -1)
                {
                    throw new System.Exception("�Ƿ���TCP��Դ��ַ��ʾ��");
                }
                String[] hostAddress = _address.Substring(0, firstFlagOffset).Split(new[] {":"},
                                                                                    StringSplitOptions.
                                                                                        RemoveEmptyEntries);
                String serviceName = _address.Substring(firstFlagOffset + 1, _address.Length - (firstFlagOffset + 1));
                if (hostAddress.Length <= 1 || String.IsNullOrEmpty(serviceName))
                {
                    throw new System.Exception("�Ƿ���TCP��Դ��ַ��ʾ��");
                }
                _hostAddress = hostAddress[0];
                _port = int.Parse(hostAddress[1]);
                _serviceName = serviceName;
                if (_port <= 0 || _port > 65535)
                {
                    throw new System.Exception("�Ƿ���TCP��Դ��ַ��ʾ��");
                }
                if (_hostAddress.ToLower() == "localhost" || _hostAddress == "127.0.0.1")
                {
                    _hostAddress = "127.0.0.1";
                    _isHost = true;
                }
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                throw new System.Exception("�Ƿ���TCP��Դ��ַ��ʾ��");
            }
        }

        #endregion
    }
}