using System.Collections.Generic;
using KJFramework.Logger;
using KJFramework.Platform.Deploy.DSN.CP.Deployer.Packages;
using KJFramework.Platform.Deploy.DSN.ProtocolStack;

namespace KJFramework.Platform.Deploy.DSN.CP.Deployer
{
    /// <summary>
    ///     数据通道，用于文件数据的集中存储
    /// </summary>
    public static class DataBus
    {
        #region Members

        private static Dictionary<string, IFilePackage> _packages = new Dictionary<string, IFilePackage>();

        #endregion

        #region Methods

        /// <summary>
        ///     添加一个文件包
        /// </summary>
        /// <param name="message">请求消息</param>
        /// <param name="reason">添加失败的原因</param>
        /// <returns>返回添加的结果</returns>
        public static bool AddPackage(DSNBeginTransferFileRequestMessage message, out string reason)
        {
            IFilePackage package;
            //Can find it.
            if (_packages.TryGetValue(message.RequestToken, out package))
            {
                reason = "Already exists.";
                return false;
            }
            try
            {
                //Try to add this file package.
                package = new FilePackage(message.RequestToken)
                {
                    ServiceName = message.ServiceName,
                    Name = message.Name,
                    Version = message.Version,
                    Description = message.Description,
                    TotalPackageCount = message.TotalPacketCount
                };
                _packages.Add(package.RequestToken, package);
                reason = null;
                return true;
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
                throw;
            }
        }

        /// <summary>
        ///     移除文件包
        /// </summary>
        /// <param name="requestToken">请求令牌</param>
        public static void RemovePackage(string requestToken)
        {
            if (string.IsNullOrEmpty(requestToken))
            {
                return;
            }
            _packages.Remove(requestToken);
        }

        /// <summary>
        ///     获取文件包
        /// </summary>
        /// <param name="requestToken">请求令牌</param>
        /// <returns>返回文件包</returns>
        public static IFilePackage GetPackage(string requestToken)
        {
            IFilePackage package;
            if (_packages.TryGetValue(requestToken, out package))
            {
                return package;
            }
            return null;
        }

        #endregion
    }
}