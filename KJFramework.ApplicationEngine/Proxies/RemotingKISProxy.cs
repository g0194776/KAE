using System;
using System.IO;
using System.Net;
using KJFramework.ApplicationEngine.Objects;
using KJFramework.Tracing;
using Newtonsoft.Json;

namespace KJFramework.ApplicationEngine.Proxies
{
    /// <summary>
    ///    KIS(KAE Information Station)远程网关
    /// </summary>
    internal class RemotingKISProxy : IRemotingKISProxy
    {
        #region Constructor.

        /// <summary>
        ///    KIS(KAE Information Station)远程网关
        /// </summary>
        /// <param name="kisAddress">KIS远程地址</param>
        public RemotingKISProxy(string kisAddress)
        {
            if (string.IsNullOrEmpty(kisAddress)) throw new ArgumentNullException("kisAddress");
            _kisAddress = kisAddress;
        }

        #endregion

        #region Members.

        private static string _kisAddress;
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(RemotingKISProxy));

        #endregion

        #region Methods.

        /// <summary>
        ///    通过一个KPP的名称向远程KAE服务站来获取真实的下载地址
        /// </summary>
        /// <param name="kppName">KPP名称</param>
        /// <param name="version">KPP包版本号</param>
        /// <returns>返回获取到的KPP详细信息</returns>
        public PackageInfo GetReallyRemotingAddress(string kppName, string version)
        {
            string reqAddr = Path.Combine(_kisAddress, string.Format("v1/information/kpp?pkgname={0}&ver={1}", kppName, version));
            try
            {
                HttpWebRequest request = WebRequest.CreateHttp(reqAddr);
                request.Method = "GET";
                request.ContentType = "application/json";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode != HttpStatusCode.OK) return null;
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    return JsonConvert.DeserializeObject<PackageInfo>(reader.ReadToEnd());
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex);
                return null;
            }
        }

        #endregion
    }
}