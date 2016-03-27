using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using KJFramework.Net.Uri;

namespace KJFramework.Net.EndPoints
{
    /// <summary>
    ///   基于命名管道通信方式的终端地址节点
    /// </summary>
    public class NamedPipeEndPoint : EndPoint
    {
        #region Memberes.

        private readonly ulong _id;
        private static readonly MD5 _md5 = new MD5CryptoServiceProvider();

        #endregion

        #region Constructor

        /// <summary>
        ///   基于命名管道通信方式的终端地址节点
        /// </summary>
        /// <param name="uri">基于命名管道形式的终端地址表现形式</param>
        /// <param name="numInstance">
        ///     当前命名管道实例数的index
        ///     <para>* index从0开始算起</para>
        /// </param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        /// <exception cref="UriFormatException">非法的终端地址格式</exception>
        public NamedPipeEndPoint(string uri, int numInstance)
            : this(new PipeUri(uri), numInstance)
        {
        }

        /// <summary>
        ///   基于命名管道通信方式的终端地址节点
        /// </summary>
        /// <param name="id">命名管道地址的唯一ID</param>
        public NamedPipeEndPoint(ulong id)
        {
            _id = id;
        }

        /// <summary>
        ///   基于命名管道通信方式的终端地址节点
        /// </summary>
        /// <param name="uri">基于命名管道形式的终端地址表现形式</param>
        /// <param name="numInstance">
        ///     当前命名管道实例数的index
        ///     <para>* index从0开始算起</para>
        /// </param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        /// <exception cref="UriFormatException">非法的终端地址格式</exception>
        public NamedPipeEndPoint(PipeUri uri, int numInstance)
        {
            if (uri == null) throw new ArgumentNullException("uri");
            byte[] source = _md5.ComputeHash(Encoding.UTF8.GetBytes(string.Format("{0}#{1}", uri, numInstance)));
            unsafe
            {
                fixed (byte* pData = source) _id = *(ulong*)(pData + 4);
            }
        }

        #endregion

        #region Methods.

        /// <summary>
        ///   获取内部命名管道地址所代表的唯一ID
        /// </summary>
        /// <returns>返回唯一ID</returns>
        public ulong GetPipeCodeId()
        {
            return _id;
        }


        #endregion
    }
}