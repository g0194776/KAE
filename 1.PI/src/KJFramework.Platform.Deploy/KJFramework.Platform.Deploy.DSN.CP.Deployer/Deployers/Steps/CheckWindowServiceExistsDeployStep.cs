using System;
using System.ServiceProcess;

namespace KJFramework.Platform.Deploy.DSN.CP.Deployer.Deployers.Steps
{
    /// <summary>
    ///     ���WINDOWS SERVICE�Ƿ�����ڱ����Ĳ�����
    /// </summary>
    public class CheckWindowServiceExistsDeployStep : DeployStep
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
            if (args == null || args.Length != 1)
            {
                throw new ArgumentException("Wrong arguments.");
            }
            string serviceName = (string) args[0];
            foreach (var service in ServiceController.GetServices())
            {
                if (service.ServiceName == serviceName)
                {
                    context = new object[] {true, serviceName};
                    return true;
                }
            }
            context = null;
            return false;
        }

        #endregion
    }
}