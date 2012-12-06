using System.Net;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.HostChannels;

namespace KJFramework.Net.Cloud.Urls
{
    /// <summary>
    ///   TCPЭ��ķ�����URI���ṩ����صĻ�������
    /// </summary>
    public class TcpServerAreaUri : ServerAreaUri
    {
        #region ���캯��

        /// <summary>
        ///   TCPЭ��ķ�����URI���ṩ����صĻ�������
        /// </summary>
        public TcpServerAreaUri()
        {
            
        }

        /// <summary>
        ///   TCPЭ��ķ�����URI���ṩ����صĻ�������
        /// </summary>
        /// <param name="address">Զ���ս���ַ</param>
        public TcpServerAreaUri(IPEndPoint address)
        {
            _address = address;
        }

        #endregion

        #region Members

        private IPEndPoint _address;
        /// <summary>
        ///   ��ȡ������Զ���ս���ַ
        /// </summary>
        public IPEndPoint Address
        {
            get { return _address; }
            set { _address = value; }
        }

        #endregion

        #region Overrides of ServerAreaUri

        /// <summary>
        ///   ����һ�������ŵ�
        /// </summary>
        /// <returns>��������ͨ��</returns>
        public override IHostTransportChannel CreateHostChannel()
        {
            return new TcpHostTransportChannel(_address.Port);
        }

        /// <summary>
        ///   ����һ��ͨѶ�ŵ�
        /// </summary>
        /// <returns>����ͨѶ�ŵ�</returns>
        public override ITransportChannel CreateTransportChannel()
        {
            return new TcpTransportChannel(_address);
        }

        #endregion
    }
}