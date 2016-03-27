using System;
using System.Net;
using System.Runtime.Remoting;
using Newtonsoft.Json;

namespace KJFramework.ApplicationEngine.Proxies
{
    /// <summary>
    ///    远程KPP下载器
    /// </summary>
    public interface IRemotingApplicationDownloader
    {
        #region Methods.

        /// <summary>
        ///    使用一个装配清单来下载远程的KPP列表
        ///     <para>* 这要求装配清单中的每一个完整的APP名称都要由以下格式来组成</para>
        ///     <para>* PackageName-Version</para>
        /// </summary>
        /// <param name="workRoot">当前应用程序所在的目录</param>
        /// <param name="installingListFile">装配清单文件</param>
        /// <returns>返回已下载后放置KPP的本地目录路径</returns>
        /// <exception cref="WebException">无法下载远程指定的KPP</exception>
        /// <exception cref="FormatException">错误的KPP包名格式</exception>
        /// <exception cref="RemotingException">与远程KIS通信失败</exception>
        /// <exception cref="JsonReaderException ">错误的JSON格式</exception>
        string DownloadFromList(string workRoot, string installingListFile);
        /// <summary>
        ///    使用一个远程可访问的地址来下载一个KPP安装包
        /// </summary>
        /// <param name="workRoot">当前应用程序所在的目录</param>
        /// <param name="url">KPP安装文件的远程可访问路径</param>
        /// <returns>返回已下载后放置KPP的本地目录路径</returns>
        /// <exception cref="WebException">无法下载远程指定的KPP</exception>
        /// <exception cref="FormatException">错误的KPP包名格式</exception>
        string DownloadFromUrl(string workRoot, string url);

        #endregion
    }
}