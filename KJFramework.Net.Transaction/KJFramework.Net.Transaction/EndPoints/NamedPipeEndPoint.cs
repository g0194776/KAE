using System.Net;
using System.Net.Sockets;
using KJFramework.Net.Channels.Uri;

namespace KJFramework.Net.Transaction.EndPoints
{
    /// <summary>
    ///   基于命名管道的终端
    /// </summary>
    public class NamedPipeEndPoint : EndPoint
    {
        #region Members.

        /// <summary>
        ///     Gets the address family to which the endpoint belongs.
        /// </summary>
        /// <returns>
        ///     One of the <see cref="T:System.Net.Sockets.AddressFamily"/> values.
        /// </returns>
        /// <exception cref="T:System.NotImplementedException">Any attempt is made to get or set the property when the property is not overridden in a descendant class. </exception><PermissionSet><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/></PermissionSet>
        public override AddressFamily AddressFamily
        {
            get
            {
                return AddressFamily.DataLink;
            }
        }

        /// <summary>
        ///   获取或设置通讯所使用的命名管道地址
        /// </summary>
        public PipeUri Uri { get; set; }

        #endregion
    }
}