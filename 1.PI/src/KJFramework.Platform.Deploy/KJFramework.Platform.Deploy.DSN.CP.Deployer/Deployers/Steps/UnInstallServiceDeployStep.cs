using System;
using KJFramework.Dynamic.Installers;

namespace KJFramework.Platform.Deploy.DSN.CP.Deployer.Deployers.Steps
{
    /// <summary>
    ///     ж�ط�������
    /// </summary>
    public class UnInstallServiceDeployStep : DeployStep
    {
        #region Overrides of DeployStep

        /// <summary>
        ///     ִ�в���
        /// </summary>
        /// <param name="context">������</param>
        /// <param name="args">��ز���</param>
        /// <returns>����ִ�еĽ��</returns>
        protected override bool InnerExecute(out object[] context, params object[] args)
        {
            if (args == null || args.Length != 2)
            {
                throw new ArgumentException("Wrong arguments.");
            }
            bool exists = (bool) args[0];
            string serviceName = (string) args[1];
            if (!exists)
            {
                _reporter.Notify("#Can not uninstall a service, because this service not existed. #name: " + serviceName + ".");
                throw new System.Exception("Can not uninstall a service, because this service not existed. #name: " + serviceName + ".");
            }
            _reporter.Notify("#Begin uninstall service #name: " + serviceName + "......");
            DynamicServiceInstaller installer = new DynamicServiceInstaller();
            bool result = installer.UnInstall(serviceName);
            _reporter.Notify(result
                                 ? "#Uninstall service successed! #name: " + serviceName
                                 : "#Uninstall service failed! #name: " + serviceName);
            context = null;
            return result;
        }

        #endregion
    }
}