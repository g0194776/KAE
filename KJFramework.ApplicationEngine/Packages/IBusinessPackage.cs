using System.Security.Cryptography.X509Certificates;
using KJFramework.ApplicationEngine.Eums;

namespace KJFramework.ApplicationEngine.Packages
{
    /// <summary>
    ///    业务包裹接口
    /// </summary>
    /// <typeparam name="T">业务包裹所支持的协议消息类型</typeparam>
    public interface IBusinessPackage<T>
    {
        #region Members.

        /// <summary>
        ///    获取当前业务包裹的状态
        /// </summary>
        BusinessPackageStates State { get; }
        /// <summary>
        ///    获取当前业务包裹所使用的协议类别
        /// </summary>
        ProtocolTypes ProtocolType { get; }
        /// <summary>
        ///    获取请求消息
        /// </summary>
        T Request { get; }
        /// <summary>
        ///    获取应答消息
        /// </summary>
        T Response { get; }

        #endregion

        #region Methods.

        /// <summary>
        ///    发送请求消息
        /// </summary>
        /// <param name="msg">请求消息</param>
        void SendRequest(T msg);
        /// <summary>
        ///    发送应答消息
        /// </summary>
        /// <param name="msg">应答消息</param>
        void SendResponse(T msg);

        #endregion
    }
}