using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using System.Net;
using System.Runtime.Remoting;
using KJFramework.ApplicationEngine.Objects;
using KJFramework.Tracing;
using Newtonsoft.Json;

namespace KJFramework.ApplicationEngine.Proxies
{
    /// <summary>
    ///    远程KPP下载器
    /// </summary>
    internal class RemotingApplicationDownloader : IRemotingApplicationDownloader
    {
        #region Members.

        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(RemotingApplicationDownloader));

        #endregion

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
        /// <exception cref="InstanceNotFoundException">无法从装配清单中解析到任何远程KPP的信息</exception>
        /// <exception cref="FormatException">错误的KPP包名格式</exception>
        /// <exception cref="RemotingException">与远程KIS通信失败</exception>
        /// <exception cref="JsonReaderException ">错误的JSON格式</exception>
        public string Download(string workRoot, string installingListFile)
        {
            _tracing.Info("\t#Opened remoting downloading package mode!");
            if (!File.Exists(installingListFile)) throw new FileNotFoundException(string.Format("#Current KAE installing package list file could not be found! {0}", installingListFile));
            _tracing.Info("\t\t#Searching local KAE package...");
            string storeDir = Path.Combine(workRoot, "Applications");
            if (!Directory.Exists(storeDir)) Directory.CreateDirectory(storeDir);
            List<string> files = Directory.GetFiles(workRoot, "*.kpp", SearchOption.AllDirectories).Select(Path.GetFileNameWithoutExtension).ToList();
            _tracing.Info("\t\t#Analyzing installing package file...");
            PackageList obj = JsonConvert.DeserializeObject<PackageList>(File.ReadAllText(installingListFile));
            if (obj == null || obj.Packages == null || obj.Packages.Count == 0)
                throw new InstanceNotFoundException(string.Format("#Could not get any information by curent KAE installing package list file: {0}", installingListFile));
            foreach (Package package in obj.Packages)
            {
                if (files.Contains(package.Name))
                {
                    _tracing.Info("\t\t\t#Skipping existed package: {0}.", package.Name);
                    continue;
                }
                string[] args = package.Name.Split(new[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                if (args.Length != 2) throw new FormatException(string.Format("#Bad format package name: {0}. It MUST be format like that. \"name-version\"", package.Name));
                PackageInfo packageInfo = ((IRemotingKISProxy) KAESystemInternalResource.Factory.GetResource(KAESystemInternalResource.KISProxy)).GetReallyRemotingAddress(args[0], args[1]);
                if (packageInfo == null) throw new RemotingException(string.Format("#Could not get any information from the remoting KIS by given package name: {0}", package.Name));
                //Just store it under the first level of folder.
                string tempFileLocation = Path.Combine(storeDir, Path.GetFileName(packageInfo.Url));
                _tracing.Info("\t\t#Downloading remoting KAE package: {0}...", packageInfo.Url);
                InnerDownload(packageInfo.Url, tempFileLocation);
            }
            return storeDir;
        }

        /// <summary>
        ///    下载指定的远程文件
        /// </summary>
        /// <param name="remotingFilePath">远程文件访问地址</param>
        /// <param name="localPath">本地文件的保存路径</param>
        /// <returns>返回一个HTTP的RSP文件流</returns>
        protected virtual void InnerDownload(string remotingFilePath, string localPath)
        {
            WebClient client = new WebClient();
            try { client.DownloadFile(remotingFilePath, localPath); }
            catch (System.Exception ex)
            {
                _tracing.Error(ex);
                throw;
            }
            finally { client.Dispose(); }
        }

        #endregion
    }
}