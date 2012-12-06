using System.Net;
using KJFramework.Data.Synchronization.Enums;

namespace KJFramework.Data.Synchronization
{
    /// <summary>
    ///     网络资源
    /// </summary>
    public class NetworkResource : INetworkResource
    {
        #region Constructor

        /// <summary>
        ///     网络资源
        /// </summary>
        public NetworkResource()
        {
            _mode = ResourceMode.Unknown;
        }

        /// <summary>
        ///     网络资源
        /// </summary>
        /// <param name="port">本地端口</param>
        public NetworkResource(int port)
        {
            Change(port);
        }

        /// <summary>
        ///     网络资源
        /// </summary>
        /// <param name="iep">
        ///     远程终结点地址
        ///     <para>* 格式: ip:port</para>
        /// </param>
        public NetworkResource(string iep)
        {
            Change(iep);
        }

        /// <summary>
        ///     网络资源
        /// </summary>
        /// <param name="iep">远程终结点地址</param>
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
        ///     获取或设置资源类型
        /// </summary>
        public ResourceMode Mode
        {
            get { return _mode; }
        }

        /// <summary>
        ///     更换资源类型
        /// </summary>
        /// <param name="port">本地要监听的端口</param>
        public void Change(int port)
        {
            if (port > IPEndPoint.MaxPort || port < IPEndPoint.MinPort)
                throw new System.ArgumentException("Illegal local TCP port! #" + port);
            _res = port;
            _mode = ResourceMode.Local; 
        }

        /// <summary>
        ///     更换资源类型
        /// </summary>
        /// <param name="iep">
        ///     远程地址
        ///     <para>* 格式: ip:port</para>
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
        ///     更换资源类型
        /// </summary>
        /// <param name="iep">远程终结点地址</param>
        public void Change(IPEndPoint iep)
        {
            if (iep == null) throw new System.ArgumentNullException("iep");
            _res = iep;
            _mode = ResourceMode.Remote; 
        }

        /// <summary>
        ///     获取网络资源
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <returns>返回内部的资源</returns>
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