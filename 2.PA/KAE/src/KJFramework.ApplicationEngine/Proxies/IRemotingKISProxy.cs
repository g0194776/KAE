using KJFramework.ApplicationEngine.Objects;

namespace KJFramework.ApplicationEngine.Proxies
{
    /// <summary>
    ///    远程KAE信息站的访问代理器接口
    /// </summary>
    internal interface IRemotingKISProxy
    {
        #region Methods.

        /// <summary>
        ///    通过一个KPP的名称向远程KAE服务站来获取真实的下载地址
        /// </summary>
        /// <param name="kppName">KPP名称</param>
        /// <param name="version">KPP包版本号</param>
        /// <returns>返回获取到的KPP详细信息</returns>
        PackageInfo GetReallyRemotingAddress(string kppName, string version);

        #endregion
    }
}