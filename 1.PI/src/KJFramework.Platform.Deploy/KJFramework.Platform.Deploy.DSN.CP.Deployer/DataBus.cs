using System.Collections.Generic;
using KJFramework.Logger;
using KJFramework.Platform.Deploy.DSN.CP.Deployer.Packages;
using KJFramework.Platform.Deploy.DSN.ProtocolStack;

namespace KJFramework.Platform.Deploy.DSN.CP.Deployer
{
    /// <summary>
    ///     ����ͨ���������ļ����ݵļ��д洢
    /// </summary>
    public static class DataBus
    {
        #region Members

        private static Dictionary<string, IFilePackage> _packages = new Dictionary<string, IFilePackage>();

        #endregion

        #region Methods

        /// <summary>
        ///     ���һ���ļ���
        /// </summary>
        /// <param name="message">������Ϣ</param>
        /// <param name="reason">���ʧ�ܵ�ԭ��</param>
        /// <returns>������ӵĽ��</returns>
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
        ///     �Ƴ��ļ���
        /// </summary>
        /// <param name="requestToken">��������</param>
        public static void RemovePackage(string requestToken)
        {
            if (string.IsNullOrEmpty(requestToken))
            {
                return;
            }
            _packages.Remove(requestToken);
        }

        /// <summary>
        ///     ��ȡ�ļ���
        /// </summary>
        /// <param name="requestToken">��������</param>
        /// <returns>�����ļ���</returns>
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