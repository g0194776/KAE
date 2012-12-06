using System.Net;
using KJFramework.Data.Synchronization.Enums;

namespace KJFramework.Data.Synchronization
{
    /// <summary>
    ///     ������Դ
    /// </summary>
    public class NetworkResource : INetworkResource
    {
        #region Constructor

        /// <summary>
        ///     ������Դ
        /// </summary>
        public NetworkResource()
        {
            _mode = ResourceMode.Unknown;
        }

        /// <summary>
        ///     ������Դ
        /// </summary>
        /// <param name="port">���ض˿�</param>
        public NetworkResource(int port)
        {
            Change(port);
        }

        /// <summary>
        ///     ������Դ
        /// </summary>
        /// <param name="iep">
        ///     Զ���ս���ַ
        ///     <para>* ��ʽ: ip:port</para>
        /// </param>
        public NetworkResource(string iep)
        {
            Change(iep);
        }

        /// <summary>
        ///     ������Դ
        /// </summary>
        /// <param name="iep">Զ���ս���ַ</param>
        public NetworkResource(IPEndPoint iep)
        {
            Change(iep);
        }

        #endregion

        #region Members

        private string _toStr;

        #endregion

        #region Implementation of INetworkResource

        private object _res;
        private ResourceMode _mode;

        /// <summary>
        ///     ��ȡ��������Դ����
        /// </summary>
        public ResourceMode Mode
        {
            get { return _mode; }
        }

        /// <summary>
        ///     ������Դ����
        /// </summary>
        /// <param name="port">����Ҫ�����Ķ˿�</param>
        public void Change(int port)
        {
            if (port > IPEndPoint.MaxPort || port < IPEndPoint.MinPort)
                throw new System.ArgumentException("Illegal local TCP port! #" + port);
            _res = port;
            _mode = ResourceMode.Local; 
        }

        /// <summary>
        ///     ������Դ����
        /// </summary>
        /// <param name="iep">
        ///     Զ�̵�ַ
        ///     <para>* ��ʽ: ip:port</para>
        /// </param>
        public void Change(string iep)
        {
            if (string.IsNullOrEmpty(iep)) throw new System.ArgumentNullException("iep");
            int offset = iep.LastIndexOf(':');
            string ip = iep.Substring(0, offset);
            int port = int.Parse(iep.Substring(offset + 1, iep.Length - (offset + 1)));
            _res = new IPEndPoint(IPAddress.Parse(ip), port);
            _mode = ResourceMode.Remote; 
        }

        /// <summary>
        ///     ������Դ����
        /// </summary>
        /// <param name="iep">Զ���ս���ַ</param>
        public void Change(IPEndPoint iep)
        {
            if (iep == null) throw new System.ArgumentNullException("iep");
            _res = iep;
            _mode = ResourceMode.Remote; 
        }

        /// <summary>
        ///     ��ȡ������Դ
        /// </summary>
        /// <typeparam name="T">��Դ����</typeparam>
        /// <returns>�����ڲ�����Դ</returns>
        public T GetResource<T>()
        {
            if (_mode == ResourceMode.Unknown) throw new System.Exception("Cannot get special network resource! #mode: " + _mode);
            return (T)_res;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            if (_toStr == null) _toStr = (_mode == ResourceMode.Local ? _res.ToString() : ((IPEndPoint)(_res)).Address.ToString());
            return _toStr;
        }

        #endregion
    }
}